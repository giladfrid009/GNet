using System;
using System.Collections.Generic;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : ConvBase
    {
        public IPooler Pooler { get; }
        public override bool IsTrainable { get => false; set => throw new NotSupportedException(); }

        public Pooling(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IPooler pooler) :
            base(inputShape, kernelShape, strides, paddings, 1)
        {
            Pooler = pooler;
        }

        protected override Shape CalcOutputShape(Shape inputShape)
        {
            return new Shape(inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * Paddings[i] - KernelShape.Dimensions[i]) / Strides[i]));
        }

        public override void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ArgumentException("InLayer shape mismatch.");
            }

            ShapedArrayImmutable<Neuron> padded = PadInNeurons(inLayer, PaddedShape);

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, i) =>
            {
                Neuron N = Neurons[i];

                N.InSynapses = IndexGen.ByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select((idx, j) =>
                {
                    var S = new Synapse(padded[idx], N);

                    inConnections[idx].Add(S);

                    return S;
                })
                .ToShape(KernelShape);
            });

            padded.ForEach((N, i) => N.OutSynapses = new ShapedArrayImmutable<Synapse>(new Shape(inConnections[i].Count), inConnections[i]));
        }

        public override void Initialize()
        {
            // TODO: TAKES A LOT OF TIME.

            Neurons.ForEach(N =>
            {
                ShapedArrayImmutable<double> weights = Pooler.GetWeights(N.InSynapses.Select(S => S.InNeuron.ActivatedValue));

                N.InSynapses.ForEach((S, i) => S.Weight = weights[i]);
            });
        }

        public override void Forward()
        {
            Initialize();

            Neurons.ForEach(N =>
            {
                N.Value = N.InSynapses.Sum(S => S.Weight * S.InNeuron.ActivatedValue);
                N.ActivatedValue = N.Value;
            });
        }

        public override void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ArgumentException("Targets shape mismatch.");
            }

            if (loss is IOutTransformer)
            {
                throw new ArgumentException($"{nameof(loss)} loss doesn't support backpropogation.");
            }

            ShapedArrayImmutable<double> grads = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public override void CalcGrads()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public override void Update()
        {
        }

        public override ILayer Clone()
        {
            return new Pooling(InputShape, KernelShape, Strides, Paddings, Pooler)
            {
                Neurons = Neurons.Select(N => N.Clone()),
                Kernels = Kernels.Select(K => K.Clone())
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using GNet.Model;
using GNet.Utils;

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

            ShapedArrayImmutable<Neuron> padded = PadInNeurons(inLayer);

            var inConnections = new ShapedArrayImmutable<ArrayBuilder<Synapse>>(PaddedShape, () => new ArrayBuilder<Synapse>());

            IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, i) =>
            {
                Neuron N = Neurons[i];

                N.InSynapses = IndexGen.ByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select((idx, j) =>
                {
                    var S = new Synapse(padded[idx], N);
                    inConnections[idx].Add(S);
                    return S;
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = inConnections[i].ToImmutable());
        }

        public override void Initialize()
        {
            Kernels[0].Bias.Value = 0;
        }

        public override void Forward()
        {
            Neurons.ForEach(N =>
            {
                N.Value = Pooler.Pool(N.InSynapses.Select(S => S.InNeuron.ActivatedValue), out ArrayImmutable<double> inWeights);

                N.ActivatedValue = N.Value;

                N.InSynapses.ForEach((S, i) => S.Weight = inWeights[i]);
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

            Neurons.ForEach((N, i) => N.Gradient = grads[i]);
        }

        public override void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public override void Update()
        {
        }
    }
}
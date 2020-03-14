using System;
using System.Collections.Generic;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : ILayer
    {
        public IPooler Pooler { get; private set; }
        public ShapedArrayImmutable<Neuron> Neurons { get; private set; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape Shape { get; private set; }
        public Shape InputShape { get; private set; }
        public Shape KernelShape { get; }
        public Shape PaddedShape { get; private set; }
        public bool IsTrainable { get; } = false;

        // todo: i dont like that it's not appearent what's the input / output shape is when we creating conv layers. maybe force to input the input shape when creating it?
        public Pooling(Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IPooler pooler)
        {
            ValidateParams(kernelShape, strides, paddings);

            KernelShape = kernelShape;
            Strides = strides;
            Paddings = paddings;
            Pooler = pooler.Clone();
        }

        private static void ValidateParams(Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            if (kernelShape.NumDimentions != strides.Length)
            {
                throw new ArgumentException("Strides dimensions count mismatch.");
            }

            if (kernelShape.NumDimentions != paddings.Length)
            {
                throw new ArgumentException("Paddings dimensions count mismatch.");
            }

            for (int i = 0; i < strides.Length; i++)
            {
                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"Strides [{i}] is out of range.");
                }

                if (paddings[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"Paddings [{i}] is out of range.");
                }
            }
        }

        private void ValidateInLayer(Shape inputShape)
        {
            if (inputShape.NumDimentions != KernelShape.NumDimentions)
            {
                throw new ArgumentException("InputShape dimensions count mismatch.");
            }

            for (int i = 0; i < Strides.Length; i++)
            {
                if (inputShape.Dimensions[i] < KernelShape.Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException($"KernelShape dimension [{i}] is out of range.");
                }

                if((inputShape.Dimensions[i] + 2 * Paddings[i] - KernelShape.Dimensions[i]) % Strides[i] != 0)
                {
                    throw new ArgumentOutOfRangeException($"Dimension [{i}] params are invalid.");
                }
            }
        }

        private Shape CalcPaddedShape(Shape inputShape)
        {
            return new Shape(inputShape.Dimensions.Select((D, i) => D + 2 * Paddings[i]));
        }

        private Shape CalcOutputShape(Shape inputShape)
        {
            ArrayImmutable<int> outDims = inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * Paddings[i] - KernelShape.Dimensions[i]) / Strides[i]);

            return new Shape(outDims);
        }

        private void InitInput(ILayer inLayer)
        {
            ValidateInLayer(inLayer.Shape);

            InputShape = inLayer.Shape;

            PaddedShape = CalcPaddedShape(inLayer.Shape);

            Shape = CalcOutputShape(inLayer.Shape);

            Neurons = new ShapedArrayImmutable<Neuron>(Shape, () => new Neuron());
        }

        public void Connect(ILayer inLayer)
        {
            InitInput(inLayer);

            var arr = Array.CreateInstance(typeof(Neuron), PaddedShape.Dimensions.ToMutable());

            ConvHelpers.IndicesByStart(InputShape, Paddings).ForEach((idx, i) => arr.SetValue(inLayer.Neurons[i], idx));

            ShapedArrayImmutable<Neuron> padded = new ShapedArrayImmutable<Neuron>(PaddedShape, arr).Select(N => N ?? new Neuron());

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            ConvHelpers.IndicesByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, i) =>
            {
                Neurons[i].InSynapses = ConvHelpers.IndicesByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select(idx =>
                {
                    var S = new Synapse(padded[idx], Neurons[i]);
                    inConnections[idx].Add(S);
                    return S;
                })
                .ToShape(KernelShape);
            });

            padded.ForEach((N, i) => N.OutSynapses = new ShapedArrayImmutable<Synapse>(new Shape(inConnections[i].Count), inConnections[i]));
        }

        public void Initialize()
        {
            Neurons.ForEach(N =>
            {
                ShapedArrayImmutable<double> weights = Pooler.GetWeights(N.InSynapses.Select(S => S.InNeuron.ActivatedValue));

                N.InSynapses.ForEach((S, i) => S.Weight = weights[i]);
            });
        }

        public void Input(ShapedArrayImmutable<double> values)
        {
            throw new NotSupportedException("This layer can't be used as input layer.");
        }

        public void Forward()
        {
            Initialize();

            Neurons.ForEach(N =>
            {
                N.Value = N.InSynapses.Sum(S => S.Weight * S.InNeuron.ActivatedValue);
                N.ActivatedValue = N.Value;
            });
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
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

        public void CalcGrads()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public virtual void Update()
        {
        }

        public virtual ILayer Clone()
        {
            return new Pooling(KernelShape, Strides, Paddings, Pooler)
            {
                Pooler = Pooler.Clone(),
                Neurons = Neurons.Select(N => N.Clone()),
                Shape = Shape,
                InputShape = InputShape,
                PaddedShape = PaddedShape
            };
        }
    }
}
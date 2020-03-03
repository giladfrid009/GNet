using System;
using System.Collections.Generic;
using GNet.Model;

namespace GNet.Layers.Internal
{
    [Serializable]
    public class PoolingOut : ILayer
    {
        public IKernel Kernel { get; protected set; }
        public ShapedArrayImmutable<Neuron> Neurons { get; protected set; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape ShapeInput { get; }
        public Shape ShapeKernel { get; }
        public Shape ShapePadded { get; }
        public Shape Shape { get; }
        public virtual bool IsTrainable { get; } = false;

        public PoolingOut(Shape shapeInput, Shape shapeKernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IKernel kernel)
        {
            ValidateParams(shapeInput, shapeKernel, strides, paddings);

            ShapeInput = shapeInput;
            ShapeKernel = shapeKernel;
            Strides = strides;
            Paddings = paddings;
            Kernel = kernel.Clone();

            ShapePadded = CalcPaddedShape(shapeInput, paddings);

            Shape = CalcOutputShape(shapeInput, shapeKernel, strides, paddings);
        }

        private void ValidateParams(Shape shapeInput, Shape shapeKernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            if (shapeInput.NumDimentions != shapeKernel.NumDimentions)
            {
                throw new ArgumentException("ShapeKernel dimensions count mismatch.");
            }

            if (shapeInput.NumDimentions != strides.Length)
            {
                throw new ArgumentException("Strides dimensions count mismatch.");
            }

            if (shapeInput.NumDimentions != paddings.Length)
            {
                throw new ArgumentException("Paddings dimensions count mismatch.");
            }

            for (int i = 0; i < strides.Length; i++)
            {
                if (shapeInput.Dimensions[i] < shapeKernel.Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException($"ShapeKernel dimension [{i}] is out of range.");
                }

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

        private Shape CalcPaddedShape(Shape shapeInput, ArrayImmutable<int> paddings)
        {
            return new Shape(shapeInput.Dimensions.Select((D, i) => D + 2 * paddings[i]));
        }

        private Shape CalcOutputShape(Shape shapeInput, Shape shapeKernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            ArrayImmutable<int> outDims = shapeInput.Dimensions.Select((D, i) => 1 + (D + 2 * paddings[i] - shapeKernel.Dimensions[i]) / strides[i]);

            return new Shape(outDims);
        }

        public void Connect(ILayer inLayer)
        {
            var arr = Array.CreateInstance(typeof(Neuron), ShapePadded.Dimensions.ToMutable());

            ShapeInput.GetIndicesFrom(Paddings).ForEach((idx, i) => arr.SetValue(inLayer.Neurons[i], idx));

            ShapedArrayImmutable<Neuron> padded = new ShapedArrayImmutable<Neuron>(ShapeInput, arr).Select(N => N ?? new Neuron());

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(padded.Shape, () => new List<Synapse>());

            ShapeInput.GetIndicesByStrides(Strides).ForEach((idxKernel, i) =>
            {
                Neurons[i].InSynapses = ShapeKernel.GetIndicesFrom(idxKernel).Select(idx =>
                {
                    var S = new Synapse(padded[idx], Neurons[i]);
                    inConnections[idx].Add(S);
                    return S;
                })
                .ToShape(ShapeKernel);
            });

            padded.ForEach((N, i) => N.OutSynapses = new ShapedArrayImmutable<Synapse>(new Shape(inConnections[i].Count), inConnections[i]));
        }

        public virtual void Initialize()
        {
            Neurons.ForEach(N =>
            {
                Kernel.Update(N.InSynapses);

                N.Bias = 0;
                N.InSynapses.ForEach((S, i) => S.Weight = Kernel.Weights[i]);
            });
        }

        public void Input(ShapedArrayImmutable<double> values)
        {
            throw new NotSupportedException("This layer can't be used as input layer.");
        }

        public virtual void Forward()
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
                throw new ArgumentException("targets shape mismatch.");
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
            throw new NotSupportedException("This layer can't be trained.");
        }

        public virtual ILayer Clone()
        {
            return new PoolingOut(ShapeInput, ShapeKernel, Strides, Paddings, Kernel)
            {
                Kernel = Kernel.Clone(),
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}

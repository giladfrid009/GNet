using System;
using System.Collections.Generic;
using GNet.Model;

namespace GNet.Layers
{
    // todo: create abstract convolutional class with all the shared methods between this class and pooling.
    [Serializable]
    public class Convolutional : ILayer
    {
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public ArrayImmutable<Kernel> Kernels { get; private set; }
        public ShapedArrayImmutable<Neuron> Neurons { get; private set; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape Shape { get; private set; }
        public Shape InputShape { get; private set; }
        public Shape PaddedShape { get; private set; }
        public Shape KernelShape { get; }
        public Shape ChannelShape { get; private set; }
        public int ChannelNum { get; }
        public bool IsTrainable { get; set; } = true;

        public Convolutional(int channelNum, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            ValidateConstructor(channelNum, kernelShape, strides, paddings);

            ChannelNum = channelNum;
            KernelShape = kernelShape;
            Strides = strides;
            Paddings = paddings;
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();

            Kernels = new ArrayImmutable<Kernel>(channelNum, () => new Kernel(kernelShape, weightInit));
        }

        private static void ValidateConstructor(int kernelsNum, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            if (kernelsNum <= 0)
            {
                throw new ArgumentOutOfRangeException("KernelsNum must be positive.");
            }

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

                if ((inputShape.Dimensions[i] + 2 * Paddings[i] - KernelShape.Dimensions[i]) % Strides[i] != 0)
                {
                    throw new ArgumentOutOfRangeException($"Dimension [{i}] params are invalid.");
                }
            }
        }

        private Shape CalcPaddedShape(Shape shapeInput)
        {
            return new Shape(shapeInput.Dimensions.Select((D, i) => D + 2 * Paddings[i]));
        }

        private Shape CalcChannelShape(Shape inputShape)
        {
            return new Shape(inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * Paddings[i] - KernelShape.Dimensions[i]) / Strides[i]));
        }

        public void Connect(ILayer inLayer)
        {
            ValidateInLayer(inLayer.Shape);

            InputShape = inLayer.Shape;

            PaddedShape = CalcPaddedShape(inLayer.Shape);

            ChannelShape = CalcChannelShape(inLayer.Shape);

            Shape = new Shape(new ArrayImmutable<int>(ChannelNum).Concat(ChannelShape.Dimensions));

            Neurons = new ShapedArrayImmutable<Neuron>(Shape, () => new Neuron());

            var arr = Array.CreateInstance(typeof(Neuron), PaddedShape.Dimensions.ToMutable());

            IndexGen.ByStart(InputShape, Paddings).ForEach((idx, i) => arr.SetValue(inLayer.Neurons[i], idx));

            ShapedArrayImmutable<Neuron> padded = new ShapedArrayImmutable<Neuron>(PaddedShape, arr).Select(N => N ?? new Neuron());

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            ArrayImmutable<ArrayImmutable<Synapse>> synapses = Kernels.Select((K, i) =>
            {
                int offset = i * ChannelShape.Volume;

                return IndexGen.ByStrides(PaddedShape, Strides, KernelShape).Select((idxKernel, j) =>
                {
                    return IndexGen.ByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select(idx =>
                    {
                        var S = new Synapse(padded[idx], Neurons[offset + j]);
                        inConnections[idx].Add(S);
                        return S;
                    });
                });
            })
            .Extract(X => X);

            Neurons.ForEach((N, i) => N.InSynapses = synapses[i].ToShape(KernelShape));

            padded.ForEach((N, i) => N.OutSynapses = new ShapedArrayImmutable<Synapse>(new Shape(inConnections[i].Count), inConnections[i]));
        }

        public void Initialize()
        {
            int inLength = KernelShape.Volume;

            Neurons.ForEach(N => N.Bias = BiasInit.Initialize(inLength, 1));

            Neurons.ForEach((N, i) =>
            {
                ShapedArrayImmutable<double> weights = Kernels[i / (Shape.Volume / ChannelNum)].Weights;

                N.InSynapses.ForEach((S, i) => S.Weight = weights[i]);
            });           
        }        

        public void Input(ShapedArrayImmutable<double> values)
        {
            throw new NotSupportedException("This layer can't be used as input layer.");
        }

        public void Forward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.ActivatedValue));

            ShapedArrayImmutable<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            ShapedArrayImmutable<double> lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            ShapedArrayImmutable<double> grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void CalcGrads()
        {
            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public virtual void Update()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Bias += N.BatchBias;
                N.BatchBias = 0.0;

                Kernels[i / ChannelShape.Volume].Update(N.InSynapses);

                N.InSynapses.ForEach(S => S.BatchWeight = 0.0);
            });

            Neurons.ForEach((N, i) =>
            {
                ShapedArrayImmutable<double> weights = Kernels[i / (Shape.Volume / ChannelNum)].Weights;

                N.InSynapses.ForEach((S, i) => S.Weight = weights[i]);
            });
        }

        public virtual ILayer Clone()
        {
            // todo: implement fully
            return new Convolutional(ChannelNum, KernelShape, Strides, Paddings, Activation, WeightInit, BiasInit)
            {
                Neurons = Neurons.Select(N => N.Clone()),
                Kernels = Kernels.Select(K => K.Clone()),
                ChannelShape = ChannelShape,
                Shape = Shape,
                InputShape = InputShape,
                PaddedShape = PaddedShape
            };
        }
    }
}
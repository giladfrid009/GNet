using System;
using System.Collections.Generic;
using GNet.Model;
using GNet.Model.Conv;

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
        public Shape InputShape { get; private set; }
        public Shape PaddedShape { get; private set; }
        public Shape KernelShape { get; }
        public Shape Shape { get; private set; }
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

            Kernels = new ArrayImmutable<Kernel>(channelNum, () => new Kernel(kernelShape));
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

        private Shape CalcOutputShape(Shape inputShape)
        {
            var channelDims = inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * Paddings[i] - KernelShape.Dimensions[i]) / Strides[i]);

            return new Shape(new ArrayImmutable<int>(ChannelNum).Concat(channelDims));
        }

        private void InitProperties(ILayer inLayer)
        {
            ValidateInLayer(inLayer.Shape);

            InputShape = inLayer.Shape;

            PaddedShape = CalcPaddedShape(inLayer.Shape);

            Shape = CalcOutputShape(inLayer.Shape);

            Neurons = new ShapedArrayImmutable<Neuron>(Shape, () => new CNeuron());
        }

        private ShapedArrayImmutable<Neuron> PadInNeurons(ILayer inLayer, Shape paddedShape)
        {
            var arr = Array.CreateInstance(typeof(Neuron), paddedShape.Dimensions.ToMutable());

            IndexGen.ByStart(inLayer.Shape, Paddings).ForEach((idx, i) => arr.SetValue(inLayer.Neurons[i], idx));

            return new ShapedArrayImmutable<Neuron>(paddedShape, arr).Select(N => N ?? new Neuron());
        }

        public void Connect(ILayer inLayer)
        {
            InitProperties(inLayer);

            ShapedArrayImmutable<Neuron> padded = PadInNeurons(inLayer, PaddedShape);

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            Kernels.ForEach((kernel, i) =>
            {
                int offset = i * Shape.Volume / ChannelNum;

                IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, j) =>
                {
                    var N = (CNeuron)Neurons[offset + j];

                    N.KernelBias = kernel.Bias;

                    N.InSynapses = IndexGen.ByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select((idx, k) =>
                    {
                        var S = new CSynapse(padded[idx], N)
                        {
                            KernelWeight = kernel.Weights[k]
                        };

                        inConnections[idx].Add(S);

                        return (Synapse)S;
                    })
                    .ToShape(KernelShape);
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = new ShapedArrayImmutable<Synapse>(new Shape(inConnections[i].Count), inConnections[i]));
        }

        public void Initialize()
        {
            int inLength = KernelShape.Volume;

            Kernels.ForEach(K =>
            {
                K.Bias.Value = BiasInit.Initialize(inLength, 1);
                K.Weights.ForEach(W => W.Value = WeightInit.Initialize(inLength, 1));
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
            Neurons.ForEach(N =>
            {
                N.Bias += N.BatchBias;
                N.BatchBias = 0.0;

                N.InSynapses.ForEach(S =>
                {
                    S.Weight += S.BatchWeight;
                    S.BatchWeight = 0.0;
                });
            });
        }

        public virtual ILayer Clone()
        {
            return new Convolutional(ChannelNum, KernelShape, Strides, Paddings, Activation, WeightInit, BiasInit)
            {
                Neurons = Neurons.Select(N => N.Clone()),
                Kernels = Kernels.Select(K => K.Clone()),
                Shape = Shape,
                InputShape = InputShape,
                PaddedShape = PaddedShape
            };
        }
    }
}
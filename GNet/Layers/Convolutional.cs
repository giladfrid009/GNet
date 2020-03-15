using System;
using System.Collections.Generic;
using GNet.Model;

namespace GNet.Layers
{
    // todo: create abstract convolutional class with all the shared methods between this class and pooling.
    //todo: implement proper convolution
    //todo: implement activation fnc
    //tood: implement bias
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
        public Shape KernelShape { get; }
        public Shape PaddedShape { get; private set; }
        public int KernelsNum { get; }
        public bool IsTrainable { get; set; } = true;

        public Convolutional(int kernelsNum, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            ValidateConstructor(kernelsNum, kernelShape, strides, paddings);

            KernelsNum = kernelsNum;
            KernelShape = kernelShape;
            Strides = strides;
            Paddings = paddings;
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();

            Kernels = new ArrayImmutable<Kernel>(kernelsNum, () => new Kernel(kernelShape, weightInit));
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
            ArrayImmutable<int> outDims = inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * Paddings[i] - KernelShape.Dimensions[i]) / Strides[i]);

            return new Shape(new ArrayImmutable<int>(KernelsNum).Concat(outDims));
        }

        public void Connect(ILayer inLayer)
        {
            ValidateInLayer(inLayer.Shape);

            InputShape = inLayer.Shape;

            PaddedShape = CalcPaddedShape(inLayer.Shape);

            Shape = CalcOutputShape(inLayer.Shape); // 1 more dimension than input shape, based on number of kernels

            Neurons = new ShapedArrayImmutable<Neuron>(Shape, () => new Neuron());

            var arr = Array.CreateInstance(typeof(Neuron), PaddedShape.Dimensions.ToMutable());

            IndexGen.ByStart(InputShape, Paddings).ForEach((idx, i) => arr.SetValue(inLayer.Neurons[i], idx));

            ShapedArrayImmutable<Neuron> padded = new ShapedArrayImmutable<Neuron>(PaddedShape, arr).Select(N => N ?? new Neuron());

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            // till this point everything is the same

            ArrayImmutable<ArrayImmutable<Synapse>> synapses = Kernels.Select((K, i) =>
            {
                int offset = i * KernelShape.Volume;

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

            // todo: implement
        }        

        public void Input(ShapedArrayImmutable<double> values)
        {
            throw new NotSupportedException("This layer can't be used as input layer.");
        }

        public void Forward()
        {
            // todo: implement

            Neurons.ForEach(N =>
            {
                N.Value = N.InSynapses.Sum(S => S.Weight * S.InNeuron.ActivatedValue) / KernelShape.Volume;
                N.ActivatedValue = N.Value;
            });
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            // todo: implement the activation

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
            // todo: implement the activation

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public virtual void Update()
        {
            //todo: implement
        }

        public virtual ILayer Clone()
        {
            return new Convolutional(KernelsNum, KernelShape, Strides, Paddings, Activation, WeightInit, BiasInit)
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
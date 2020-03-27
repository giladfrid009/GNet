using System;
using GNet.Model;
using GNet.Model.Convolutional;

namespace GNet.Layers
{
    [Serializable]
    public abstract class ConvBase : ILayer
    {
        public ArrayImmutable<Kernel> Kernels { get; protected set; }
        public ShapedArrayImmutable<Neuron> Neurons { get; protected set; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public Shape Shape => Neurons.Shape;
        public int KernelsNum => Kernels.Length;
        public abstract bool IsTrainable { get; set; }

        public ConvBase(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, int nKernels)
        {
            ValidateConstructor(inputShape, kernelShape, nKernels, strides, paddings);

            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            Paddings = paddings;

            PaddedShape = CalcPaddedShape(inputShape);

            Kernels = new ArrayImmutable<Kernel>(nKernels, () => new Kernel(kernelShape));

            Neurons = new ShapedArrayImmutable<Neuron>(CalcOutputShape(inputShape), () => new CNeuron());
        }

        protected static void ValidateConstructor(Shape inputShape, Shape kernelShape, int nKernels, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            if (nKernels < 1)
            {
                throw new ArgumentOutOfRangeException("nKernels nust be positive.");
            }

            if (inputShape.NumDimentions != kernelShape.NumDimentions)
            {
                throw new ArgumentException("KernelShape dimensions count mismatch.");
            }

            if (inputShape.NumDimentions != strides.Length)
            {
                throw new ArgumentException("Strides dimensions count mismatch.");
            }

            if (inputShape.NumDimentions != paddings.Length)
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

                if (inputShape.Dimensions[i] < kernelShape.Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException($"KernelShape dimension [{i}] is out of range.");
                }

                if ((inputShape.Dimensions[i] + 2 * paddings[i] - kernelShape.Dimensions[i]) % strides[i] != 0)
                {
                    throw new ArgumentOutOfRangeException($"Dimension [{i}] params are invalid.");
                }
            }
        }

        protected Shape CalcPaddedShape(Shape shapeInput)
        {
            return new Shape(shapeInput.Dimensions.Select((D, i) => D + 2 * Paddings[i]));
        }

        protected ShapedArrayImmutable<Neuron> PadInNeurons(ILayer inLayer, Shape paddedShape)
        {
            var arr = Array.CreateInstance(typeof(Neuron), paddedShape.Dimensions.ToMutable());

            IndexGen.ByStart(inLayer.Shape, Paddings).ForEach((idx, i) => arr.SetValue(inLayer.Neurons[i], idx));

            return new ShapedArrayImmutable<Neuron>(paddedShape, arr).Select(N => N ?? new Neuron());
        }

        protected abstract Shape CalcOutputShape(Shape inputShape);

        public void Input(ShapedArrayImmutable<double> values)
        {
            throw new NotSupportedException("This layer can't be used as input layer.");
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

        public abstract void Connect(ILayer inLayer);

        public abstract void Initialize();

        public abstract void Forward();

        public abstract void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets);

        public abstract void CalcGrads();

        public abstract ILayer Clone();
    }
}
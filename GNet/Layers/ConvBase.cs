using System;
using GNet.Model;
using GNet.Model.Conv;

namespace GNet.Layers
{
    [Serializable]
    public abstract class ConvBase : ILayer
    {
        public ArrayImmutable<Kernel> Kernels { get; private set; }
        public ShapedArrayImmutable<Neuron> Neurons { get; private set; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape InputShape { get; private set; }
        public Shape PaddedShape { get; private set; }
        public Shape KernelShape { get; }
        public Shape Shape { get; private set; }
        public abstract bool IsTrainable { get; set; }

        protected static void ValidateConstructor(Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
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

        protected void ValidateInLayer(Shape inputShape)
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

        protected Shape CalcPaddedShape(Shape shapeInput)
        {
            return new Shape(shapeInput.Dimensions.Select((D, i) => D + 2 * Paddings[i]));
        }

        protected abstract Shape CalcOutputShape(Shape inputShape);

        protected void InitProperties(ILayer inLayer)
        {
            ValidateInLayer(inLayer.Shape);

            InputShape = inLayer.Shape;

            PaddedShape = CalcPaddedShape(inLayer.Shape);

            Shape = CalcOutputShape(inLayer.Shape);

            Neurons = new ShapedArrayImmutable<Neuron>(Shape, () => new CNeuron());
        }

        protected ShapedArrayImmutable<Neuron> PadInNeurons(ILayer inLayer, Shape paddedShape)
        {
            var arr = Array.CreateInstance(typeof(Neuron), paddedShape.Dimensions.ToMutable());

            IndexGen.ByStart(inLayer.Shape, Paddings).ForEach((idx, i) => arr.SetValue(inLayer.Neurons[i], idx));

            return new ShapedArrayImmutable<Neuron>(paddedShape, arr).Select(N => N ?? new Neuron());
        }     

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
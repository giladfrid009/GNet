using System;
using GNet.Layers.Internal;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : ILayer
    {
        public ConvIn InternalInLayer { get; protected set; }
        public PoolingOut InternalOutLayer { get; protected set; }
        public IKernel Kernel => InternalOutLayer.Kernel;
        public ShapedArrayImmutable<Neuron> Neurons => InternalOutLayer.Neurons;
        public ArrayImmutable<int> Paddings => InternalOutLayer.Paddings;
        public ArrayImmutable<int> Strides => InternalOutLayer.Strides;
        public Shape Shape => InternalOutLayer.Shape;
        public Shape InputShape => InternalOutLayer.InputShape;
        public Shape KernelShape => InternalOutLayer.KernelShape;
        public Shape PaddedShape => InternalOutLayer.PaddedShape;
        public bool IsTrainable => InternalOutLayer.IsTrainable;

        public Pooling(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IKernel kernel)
        {
            InternalInLayer = new ConvIn(inputShape);
            InternalOutLayer = new PoolingOut(inputShape, kernelShape, strides, paddings, kernel);
        }

        public void Connect(ILayer inLayer)
        {
            InternalInLayer.Connect(inLayer);
            InternalOutLayer.Connect(InternalInLayer);
        }

        public void Initialize()
        {
            InternalInLayer.Initialize();
            InternalOutLayer.Initialize();
        }

        public void Input(ShapedArrayImmutable<double> values)
        {
            InternalInLayer.Input(values);
            InternalOutLayer.Forward();
        }

        public void Forward()
        {
            InternalInLayer.Forward();
            InternalOutLayer.Forward();
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            InternalOutLayer.CalcGrads(loss, targets);
            InternalInLayer.CalcGrads();
        }

        public void CalcGrads()
        {
            InternalOutLayer.CalcGrads();
            InternalInLayer.CalcGrads();
        }

        public virtual void Update()
        {
            throw new NotSupportedException("This layer can't be trained.");
        }

        public virtual ILayer Clone()
        {
            return new Pooling(InputShape, KernelShape, Strides, Paddings, Kernel)
            {
                InternalInLayer = (ConvIn)InternalInLayer.Clone(),
                InternalOutLayer = (PoolingOut)InternalOutLayer.Clone()
            };
        }
    }
}
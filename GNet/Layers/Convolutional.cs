using System;
using GNet.Layers.Internal;
using GNet.Layers.Kernels;

namespace GNet.Layers
{
    [Serializable]
    public class Convolutional : Pooling
    {
        public IInitializer Initializer { get; }

        public Convolutional(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IInitializer initializer) :
            base(inputShape, kernelShape, strides, paddings, new Filter(kernelShape, initializer))
        {
            InternalOutLayer = new ConvOut(inputShape, kernelShape, strides, paddings, (Filter)Kernel);

            Initializer = initializer.Clone();
        }

        public override void Update()
        {
            InternalOutLayer.Update();
        }

        public override ILayer Clone()
        {
            return new Convolutional(InputShape, KernelShape, Strides, Paddings, Initializer)
            {
                InternalInLayer = (ConvIn)InternalInLayer.Clone(),
                InternalOutLayer = (PoolingOut)InternalOutLayer.Clone()
            };
        }
    }
}
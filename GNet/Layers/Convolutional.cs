using System;
using GNet.Layers.Internal;
using GNet.Layers.Kernels;

namespace GNet.Layers
{
    [Serializable]
    public class Convolutional : Pooling
    {
        public IInitializer Initializer { get; }
        public override bool IsTrainable { get; } = true;

        public Convolutional(Shape shapeInput, Shape shapeKernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IInitializer initializer) : 
            base(shapeInput, shapeKernel, strides, paddings, new Filter(shapeKernel, initializer))
        {
            InternOutLayer = new ConvOut(shapeInput, shapeKernel, strides, paddings, (Filter)Kernel);

            Initializer = initializer.Clone();   
        }

        public override void Update()
        {
            InternOutLayer.Update();
        }

        public override ILayer Clone()
        {
            return new Convolutional(ShapeInput, ShapeKernel, Strides, Paddings, Initializer)
            {
                InternInLayer = (ConvIn)InternInLayer.Clone(),
                InternOutLayer = (PoolingOut)InternOutLayer.Clone(),
                Kernel = Kernel.Clone()
            };
        }
    }
}

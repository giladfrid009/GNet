using System;
using GNet.Layers.Internal;

namespace GNet.Layers
{
    [Serializable]
    public class Convolutional : Pooling
    {
        public IInitializer WeightInit => ((Kernels.Filter)InternalOutLayer.Kernel).WeightInit;

        protected Convolutional(ConvIn internalInLayer, ConvOut internalOutLayer) : base(internalInLayer, internalOutLayer)
        {
        }

        public Convolutional(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IInitializer weightInit) :
            this(new ConvIn(inputShape), new ConvOut(inputShape, kernelShape, strides, paddings, new Kernels.Filter(weightInit)))
        {
        }
        
        public override void Update()
        {
            InternalOutLayer.Update();
        }       

        public override ILayer Clone()
        {
            var layer =  new Convolutional((ConvIn)InternalInLayer.Clone(), (ConvOut)InternalOutLayer.Clone());

            layer.CopySynapses(this);

            return layer;
        }
    }
}
using System;
using NCollections;

namespace GNet.Layers
{
    [Serializable]
    public abstract class MergeLayer : ConstantLayer
    {
        protected MergeLayer(Shape shape) : base(shape)
        {
        }

        public abstract void Connect(Array<Layer> inLayers);

        public sealed override void Connect(Layer inLayer)
        {
            Connect(new Array<Layer>(inLayer));
        }
    }
}
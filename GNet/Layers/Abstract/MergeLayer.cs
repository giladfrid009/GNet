using System;

namespace GNet.Layers
{
    [Serializable]
    public abstract class MergeLayer : ConstantLayer
    {
        protected MergeLayer(Shape shape) : base(shape)
        {
        }

        public abstract void Connect(ImmutableArray<Layer> inLayers);

        public sealed override void Connect(Layer inLayer)
        {
            Connect(new ImmutableArray<Layer>(inLayer));
        }
    }
}
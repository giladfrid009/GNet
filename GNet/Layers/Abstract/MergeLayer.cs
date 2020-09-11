using System;

namespace GNet.Layers
{
    /// <summary>
    /// Base class for a layer that merges several layers into one.
    /// </summary>
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
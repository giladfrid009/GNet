using System;
using GNet.Model;
using GNet.Layers.Internal;

namespace GNet.Layers
{    
    [Serializable]
    public class Pooling : ILayer
    {
        public ConvIn InternInLayer { get; protected set; }
        public PoolingOut InternOutLayer { get; protected set; }
        public IKernel Kernel { get; protected set; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape ShapeInput { get; }
        public Shape ShapeKernel { get; }
        public Shape ShapePadded { get; }
        public virtual bool IsTrainable { get; } = false;

        public ShapedArrayImmutable<Neuron> Neurons => InternOutLayer.Neurons;
        public Shape Shape => InternOutLayer.Shape;

        public Pooling(Shape shapeInput, Shape shapeKernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IKernel kernel)
        {
            ShapeInput = shapeInput;
            ShapeKernel = shapeKernel;
            Strides = strides;
            Paddings = paddings;
            Kernel = kernel.Clone();

            InternInLayer = new ConvIn(ShapeInput);
            InternOutLayer = new PoolingOut(ShapeInput, ShapeKernel, Strides, Paddings, Kernel);

            ShapePadded = InternOutLayer.ShapePadded;
        }

        public void Initialize()
        {
            InternInLayer.Initialize();
            InternOutLayer.Initialize();
        }

        public void Connect(ILayer inLayer)
        {
            InternInLayer.Connect(inLayer);
            InternOutLayer.Connect(InternInLayer);
        }

        public void Input(ShapedArrayImmutable<double> values)
        {
            InternInLayer.Input(values);
            InternOutLayer.Forward();
        }

        public void Forward()
        {
            InternInLayer.Forward();
            InternOutLayer.Forward();
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            InternOutLayer.CalcGrads(loss, targets);
            InternInLayer.CalcGrads();
        }

        public void CalcGrads()
        {
            InternOutLayer.CalcGrads();
            InternInLayer.CalcGrads();
        }

        public virtual void Update()
        {
            throw new NotSupportedException("This layer can't be trained.");
        }

        public virtual ILayer Clone()
        {
            return new Pooling(ShapeInput, ShapeKernel, Strides, Paddings, Kernel)
            {
                InternInLayer = (ConvIn)InternInLayer.Clone(),
                InternOutLayer = (PoolingOut)InternOutLayer.Clone(),
                Kernel = Kernel.Clone()
            };
        }
    }
}

using GNet.Model;
using System;

namespace GNet
{
    [Serializable]
    public abstract class Layer
    {
        public Shape Shape { get; }
        public abstract Array<Neuron> Neurons { get; }

        protected Layer(Shape shape)
        {
            Shape = shape;
        }

        public void Input(ShapedArray<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            Neurons.ForEach((N, i) => N.OutVal = values[i]);
        }

        public abstract void Connect(Layer inLayer);

        public abstract void Initialize();

        public abstract void Forward(bool isTraining);

        public abstract void CalcGrads(ILoss loss, ShapedArray<double> targets);

        public abstract void CalcGrads();

        public abstract void Optimize(IOptimizer optimizer);

        public abstract void Update();
    }
}
using GNet.Model;
using System;

namespace GNet
{
    [Serializable]
    public abstract class Layer
    {
        public Shape Shape { get; }
        public abstract ImmutableArray<Neuron> Neurons { get; }

        protected Layer(in Shape shape)
        {
            Shape = shape;
        }

        public void Input(in ImmutableShapedArray<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            for (int i = 0; i < values.Length; i++)
            {
                Neurons[i].OutVal = values[i];
            }
        }

        public abstract void Connect(Layer inLayer);

        public abstract void Initialize();

        public abstract void Forward(bool isTraining);

        public abstract void CalcGrads(ILoss loss, in ImmutableShapedArray<double> targets);

        public abstract void CalcGrads();

        public abstract void Optimize(IOptimizer optimizer);

        public abstract void Update();
    }
}
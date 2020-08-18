using GNet.Model;
using NCollections;
using System;

namespace GNet
{
    /// <summary>
    /// Base layer class.
    /// </summary>
    [Serializable]
    public abstract class Layer
    {
        public Shape Shape { get; }
        public abstract Array<Neuron> Neurons { get; }

        protected Layer(Shape shape)
        {
            Shape = shape;
        }

        public void Input(NArray<double> values)
        {
            Neurons.ForEach((N, i) => N.OutVal = values[i]);
        }

        public abstract void Connect(Layer inLayer);

        public abstract void Initialize();

        public abstract void Forward(bool isTraining);

        public abstract void CalcGrads(ILoss loss, NArray<double> targets);

        public abstract void CalcGrads();

        public abstract void Optimize(IOptimizer optimizer);

        public abstract void Update();
    }
}
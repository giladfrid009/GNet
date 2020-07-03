using GNet.Model;

namespace GNet
{
    public interface ILayer
    {
        ImmutableArray<Neuron> Neurons { get; }
        Shape Shape { get; }

        public void Input(ImmutableShapedArray<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            Neurons.ForEach((N, i) => N.OutVal = values[i]);
        }

        void Connect(ILayer inLayer);

        void Initialize();

        void Forward(bool isTraining);

        void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets);

        void CalcGrads();

        void Optimize(IOptimizer optimizer);

        void Update();
    }
}
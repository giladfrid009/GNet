using GNet.Model;

namespace GNet
{
    public interface ILayer
    {
        ImmutableArray<Neuron> Neurons { get; }
        Shape Shape { get; }

        void Connect(ILayer inLayer);

        void Initialize();

        void Input(ImmutableShapedArray<double> values, bool isTraining);

        void Forward(bool isTraining);

        void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets);

        void CalcGrads();

        void Optimize(IOptimizer optimizer);

        void Update();
    }
}
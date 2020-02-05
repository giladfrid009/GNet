using GNet.Model;

namespace GNet
{
    public interface ILayer : ICloneable<ILayer>
    {
        ShapedArrayImmutable<InNeuron> InNeurons { get; }
        ShapedArrayImmutable<OutNeuron> OutNeurons { get; }
        Shape InputShape { get; }
        Shape OutputShape { get; }

        void Connect(ILayer inLayer);

        void Initialize();

        void Input(ShapedArrayImmutable<double> values);

        void Forward();

        void CalcGrads();

        void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets);

        void Update();

    }
}

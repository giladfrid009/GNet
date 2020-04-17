using static System.Math;

namespace GNet.Losses.Categorical
{
    public class Hinge : ILoss
    {
        public double Margin { get; }

        public Hinge(double margin = 1.0)
        {
            Margin = margin;
        }

        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return outputs.Select((O, i) => outputs.Select((otherO, j) => i == j ? 0.0 : Max(0.0, Margin - O + otherO)).Sum()).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            //todo: implement
        }
    }
}

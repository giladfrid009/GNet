namespace GNet
{
    public interface ILoss : IMetric
    {
        double Evaluate(double T, double O);

        double Derivative(double T, double O);

        double IMetric.Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int i = 0;
            return targets.Avarage(T => Evaluate(T, outputs[i++]));
        }
    }
}
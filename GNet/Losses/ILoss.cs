using NCollections;

namespace GNet
{
    public interface ILoss : IMetric
    {
        double Evaluate(double T, double O);

        double Derivative(double T, double O);

        double IMetric.Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            return targets.Average(outputs, (T, O) => Evaluate(T, O));
        }
    }
}
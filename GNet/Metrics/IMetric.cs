using NCollections;

namespace GNet
{
    public interface IMetric
    {
        double Evaluate(Array<double> targets, Array<double> outputs);
    }
}
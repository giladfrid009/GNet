using NCollections;

namespace GNet
{
    public interface IMetric
    {
        double Evaluate(NArray<double> targets, NArray<double> outputs);
    }
}
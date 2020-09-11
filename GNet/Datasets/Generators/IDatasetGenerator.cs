using NCollections;

namespace GNet.Datasets
{
    public interface IDatasetGenerator
    {
        Shape InputShape { get; }
        Shape TargetShape { get; }

        Dataset Generate(int length);
    }
}
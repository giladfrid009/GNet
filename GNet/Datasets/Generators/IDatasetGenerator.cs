namespace GNet.Datasets
{
    public interface IDatasetGenerator
    {
        Shape InputShape { get; }
        Shape OutputShape { get; }

        Dataset Generate(int length);
    }
}
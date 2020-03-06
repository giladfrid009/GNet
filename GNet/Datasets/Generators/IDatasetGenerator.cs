namespace GNet.Datasets
{
    public interface IDatasetGenerator : ICloneable<IDatasetGenerator>
    {
        Shape InputShape { get; }
        Shape OutputShape { get; }

        Dataset Generate(int length);
    }
}
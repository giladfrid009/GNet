namespace GNet.Datasets
{
    public interface IDatasetGenerator : ICloneable<IDatasetGenerator>
    {
        Dataset Generate(int length);
    }
}
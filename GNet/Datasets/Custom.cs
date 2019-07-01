using GNet.Extensions.Generic;

namespace GNet.Datasets
{
    public class Custom : IDataset
    {
        public Data[] DataCollection { get; } = new Data[0];
        public int DataLength { get; }
        public int InputLength { get; }
        public int TargetLength { get; }

        public Custom(Data[] dataCollection)
        {
            DataCollection = dataCollection.Select(D => D.Clone());

            DataLength = DataCollection.Length;
            InputLength = DataCollection[0].Inputs.Length;
            TargetLength = DataCollection[0].Targets.Length;
        }

        public IDataset Clone() => new Custom(DataCollection);
    }
}

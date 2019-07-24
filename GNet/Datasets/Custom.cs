using GNet.Extensions.Generic;
using System;

namespace GNet.Datasets
{
    [Serializable]
    public class Custom : IDataset
    {
        public Data[] DataCollection { get; }
        public int DataLength { get; }
        public int InputLength { get; }
        public int OutputLength { get; }

        public Custom(Data[] dataCollection)
        {
            int inputLength = DataCollection[0].Inputs.Length;
            int outputLength = DataCollection[0].Outputs.Length;

            dataCollection.ForEach((D, i) =>
            {
                if (D.Inputs.Length != inputLength || D.Outputs.Length != outputLength)
                    throw new ArgumentException($"DataCollection[{i}] structure mismatch.");
            });

            DataCollection = dataCollection.Select(D => D.Clone());

            DataLength = DataCollection.Length;
        }

        public void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection.ForEach(D => D.Normalize(inputNormalizer, outputNormalizer));
        }

        public IDataset Clone() => new Custom(DataCollection);
    }
}

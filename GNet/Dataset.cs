using System;
using GNet.Extensions.Generic;

namespace GNet
{
    [Serializable]
    public class Dataset : ICloneable<Dataset>
    {
        public Data[] DataCollection { get; }
        public int DataLength { get; }
        public int InputLength { get; }
        public int OutputLength { get; }

        public Dataset(Data[] dataCollection)
        {
            InputLength = dataCollection[0].Inputs.Length;
            OutputLength = dataCollection[0].Outputs.Length;

            dataCollection.ForEach((D, i) =>
            {
                if (D.Inputs.Length != InputLength || D.Outputs.Length != OutputLength)
                    throw new ArgumentException($"DataCollection[{i}] structure mismatch.");
            });

            DataLength = dataCollection.Length;
            DataCollection = dataCollection.Select(D => D.Clone());
        }

        public void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection.ForEach(D => D.Normalize(inputNormalizer, outputNormalizer));
        }

        public Dataset Clone() => new Dataset(DataCollection);
    }
}

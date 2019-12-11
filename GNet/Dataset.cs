using GNet.Extensions.Generic;
using System;

namespace GNet
{
    [Serializable]
    public class Dataset : ICloneable<Dataset>
    {
        public Data[] DataCollection { get; private set; }
        public int Length { get; }
        public int InputLength { get; }
        public int OutputLength { get; }

        public Data this[int index] => DataCollection[index];

        public Dataset(Data[] dataCollection)
        {
            InputLength = dataCollection[0].Inputs.Length;
            OutputLength = dataCollection[0].Outputs.Length;

            dataCollection.ForEach((D, i) =>
            {
                if (D.Inputs.Length != InputLength || D.Outputs.Length != OutputLength)
                {
                    throw new ArgumentException($"DataCollection[{i}] structure mismatch.");
                }
            });

            Length = dataCollection.Length;
            DataCollection = dataCollection.Select(D => D.Clone());
        }

        public void Normalize(INormalizer normalizer)
        {
            normalizer.ExtractParams(this);

            DataCollection = DataCollection.Select(D => NormalizeData(D, normalizer));
        }

        private Data NormalizeData(Data data, INormalizer normalizer)
        {
            var inputs = normalizer.NormalizeInputs ? normalizer.Normalize(data.Inputs) : data.Inputs;
            var outptus = normalizer.NormalizeInputs ? normalizer.Normalize(data.Outputs) : data.Outputs;

            return new Data(inputs, outptus);
        }

        public Dataset Clone()
        {
            return new Dataset(DataCollection);
        }
    }
}

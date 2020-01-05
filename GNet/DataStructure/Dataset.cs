using GNet.Extensions.Array;
using GNet.GlobalRandom;
using System;

namespace GNet
{
    public class Dataset : IArray<Data>, ICloneable<Dataset>
    {
        public int Length { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }

        private Data[] dataCollection;

        public Data this[int index] => dataCollection[index];

        public Dataset(Data[] dataCollection)
        {
            InputShape = dataCollection[0].Inputs.Shape;
            OutputShape = dataCollection[0].Outputs.Shape;

            dataCollection.ForEach((D, i) =>
            {
                if (D.Inputs.Shape != InputShape || D.Outputs.Shape != OutputShape)
                {
                    throw new ArgumentException($"DataCollection[{i}] structure mismatch.");
                }
            });

            Length = dataCollection.Length;
            this.dataCollection = dataCollection.Select(D => D.Clone());
        }

        public void Shuffle()
        {
            dataCollection.ForEach((D, i) =>
            {
                int iRnd = GRandom.Next(i, Length);
                dataCollection[i] = dataCollection[iRnd];
                dataCollection[iRnd] = D;
            });
        }

        public void Normalize(INormalizer normalizer)
        {
            normalizer.ExtractParams(this);

            dataCollection = dataCollection.Select(D => NormalizeData(D, normalizer));
        }

        private Data NormalizeData(Data data, INormalizer normalizer)
        {
            var inputs = normalizer.NormalizeInputs ? normalizer.Normalize(data.Inputs) : data.Inputs;
            var outptus = normalizer.NormalizeInputs ? normalizer.Normalize(data.Outputs) : data.Outputs;

            return new Data(inputs, outptus);
        }

        public Dataset Clone()
        {
            return new Dataset(dataCollection);
        }
    }
}

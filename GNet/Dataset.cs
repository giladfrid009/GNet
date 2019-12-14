using GNet.Extensions.Array.Generic;
using System;

namespace GNet
{
    public class Dataset : ICloneable<Dataset>
    {
        public Data[] DataCollection { get; private set; }
        public int Length { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }

        public Data this[int index] => DataCollection[index];

        public Dataset(Data[] dataCollection)
        {
            InputShape = dataCollection[0].Inputs.Shape;
            OutputShape = dataCollection[0].Outputs.Shape;

            dataCollection.ForEach((D, i) =>
            {
                if (D.Inputs.Shape.Equals(InputShape) == false || D.Outputs.Shape.Equals(OutputShape) == false)
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

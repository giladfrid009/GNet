using System;
using GNet.GlobalRandom;

namespace GNet
{
    public class Dataset : ICloneable<Dataset>
    {
        public int Length { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public ArrayImmutable<Data> DataCollection { get; private set; }

        public Dataset(ArrayImmutable<Data> dataCollection)
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
            DataCollection = dataCollection;
        }

        public Dataset(params Data[] dataCollection) : this(new ArrayImmutable<Data>(dataCollection))
        {

        }

        public void Shuffle()
        {
            Data[] shuffled = DataCollection.ToMutable();

            for (int i = 0; i < shuffled.Length; i++)
            {
                int iRnd = GRandom.Next(i++, Length);
                Data temp = shuffled[i];
                shuffled[i] = shuffled[iRnd];
                shuffled[iRnd] = temp;
            }

            DataCollection = new ArrayImmutable<Data>(shuffled);
        }

        public void Normalize(INormalizer normalizer)
        {
            normalizer.ExtractParams(this);

            DataCollection = DataCollection.Select(D => NormalizeData(D, normalizer));
        }

        private static Data NormalizeData(Data data, INormalizer normalizer)
        {
            ShapedArrayImmutable<double> inputs = normalizer.NormalizeInputs ? normalizer.Normalize(data.Inputs) : data.Inputs;
            ShapedArrayImmutable<double> outptus = normalizer.NormalizeInputs ? normalizer.Normalize(data.Outputs) : data.Outputs;

            return new Data(inputs, outptus);
        }

        public Dataset Clone()
        {
            return new Dataset(DataCollection);
        }
    }
}

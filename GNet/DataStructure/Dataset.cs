using System;

namespace GNet
{
    public class Dataset : IArray<Data>, ICloneable<Dataset>
    {
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }
        public Data this[int index] => dataCollection[index];
        private ArrayImmutable<Data> dataCollection;

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
            this.dataCollection = dataCollection;
        }

        public Dataset(params Data[] dataCollection) : this(new ArrayImmutable<Data>(dataCollection))
        {
        }

        private Data NormalizeData(Data data, INormalizer normalizer)
        {
            ShapedArrayImmutable<double> inputs = normalizer.NormalizeInputs ? normalizer.Normalize(data.Inputs) : data.Inputs;
            ShapedArrayImmutable<double> outptus = normalizer.NormalizeInputs ? normalizer.Normalize(data.Outputs) : data.Outputs;

            return new Data(inputs, outptus);
        }

        public void Normalize(INormalizer normalizer)
        {
            normalizer.ExtractParams(this);

            dataCollection = dataCollection.Select(D => NormalizeData(D, normalizer));
        }

        public void Shuffle()
        {
            Data[] shuffled = dataCollection.ToMutable();

            for (int i = 0; i < shuffled.Length; i++)
            {
                int iRnd = GRandom.Next(i++, Length);
                Data temp = shuffled[i];
                shuffled[i] = shuffled[iRnd];
                shuffled[iRnd] = temp;
            }

            dataCollection = new ArrayImmutable<Data>(shuffled);
        }

        public Dataset Clone()
        {
            return new Dataset(dataCollection);
        }
    }
}
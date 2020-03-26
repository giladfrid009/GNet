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

        public void Normalize(INormalizer? inputNormalizer, INormalizer? outputNormalizer)
        {
            inputNormalizer ??= new Normalizers.None();
            outputNormalizer ??= new Normalizers.None();

            inputNormalizer.ExtractParams(dataCollection.Select(D => D.Inputs));
            outputNormalizer.ExtractParams(dataCollection.Select(D => D.Outputs));

            dataCollection = dataCollection.Select(D => new Data(inputNormalizer.Normalize(D.Inputs), outputNormalizer.Normalize(D.Outputs)));
        }

        public void Shuffle()
        {
            Data[] shuffled = dataCollection.ToMutable();

            for (int i = 0; i < shuffled.Length; i++)
            {
                int iRnd = GRandom.Next(i, Length);
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
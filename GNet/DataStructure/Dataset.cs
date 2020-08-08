using System;

namespace GNet
{
    [Serializable]
    public class Dataset
    {
        public ImmutableArray<Data> DataCollection { get; private set; }
        public Shape InputShape { get; }
        public Shape TargetShape { get; }
        public int Length { get; }

        public Dataset(in ImmutableArray<Data> dataCollection)
        {
            if (dataCollection.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dataCollection));
            }

            InputShape = dataCollection[0].InputShape;
            TargetShape = dataCollection[0].TargetShape;

            dataCollection.ForEach((D, i) =>
            {
                if (D.InputShape != InputShape || D.TargetShape != TargetShape)
                {
                    throw new ShapeMismatchException($"{nameof(dataCollection)} [{i}] mismatch.");
                }
            });

            DataCollection = dataCollection;
            Length = dataCollection.Length;
        }

        public Dataset(params Data[] dataCollection) : this(new ImmutableArray<Data>(dataCollection))
        {
        }

        public void Normalize(INormalizer normalizer, bool inputs, bool targets)
        {
            if (!inputs && !targets)
            {
                return;
            }

            normalizer.UpdateParams(this, inputs, targets);

            DataCollection = DataCollection.Select(D => new Data(
                inputs ? D.Inputs.Select(X => normalizer.Normalize(X)) : D.Inputs,
                targets ? D.Targets.Select(X => normalizer.Normalize(X)) : D.Targets));
        }

        public void Shuffle()
        {
            Data[] shuffled = DataCollection.ToMutable();

            for (int i = 0; i < Length; i++)
            {
                int iRnd = Utils.GRandom.Next(i, Length);
                Data temp = shuffled[i];
                shuffled[i] = shuffled[iRnd];
                shuffled[iRnd] = temp;
            }

            DataCollection = ImmutableArray<Data>.FromRef(shuffled);
        }
    }
}
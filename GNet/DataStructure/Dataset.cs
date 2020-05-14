﻿using System;

namespace GNet
{
    [Serializable]
    public class Dataset : IArray<Data>
    {
        public Shape InputShape { get; }
        public Shape TargetShape { get; }
        public int Length { get; }

        public Data this[int index] => dataCollection[index];

        private ImmutableArray<Data> dataCollection;

        public Dataset(ImmutableArray<Data> dataCollection)
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

            this.dataCollection = dataCollection;
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

            dataCollection = dataCollection.Select(D => new Data(
                inputs ? D.Inputs.Select(X => normalizer.Normalize(X)).ToShape(InputShape) : D.Inputs,
                targets ? D.Targets.Select(X => normalizer.Normalize(X)).ToShape(TargetShape) : D.Targets));
        }

        public void Shuffle()
        {
            Data[] shuffled = dataCollection.ToMutable();

            for (int i = 0; i < Length; i++)
            {
                int iRnd = Utils.GRandom.Next(i, Length);
                Data temp = shuffled[i];
                shuffled[i] = shuffled[iRnd];
                shuffled[iRnd] = temp;
            }

            dataCollection = ImmutableArray<Data>.FromRef(shuffled);
        }
    }
}
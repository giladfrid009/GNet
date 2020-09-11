using System;

namespace GNet
{
    [Serializable]
    public class Dataset : Array<Data>
    {
        public Shape InputShape { get; }
        public Shape TargetShape { get; }

        protected Dataset(Data[] array, bool asRef = false) : base(array, asRef)
        {
            if (array.Length == 0)
            {
                throw new RankException(nameof(array));
            }

            InputShape = array[0].InputShape;
            TargetShape = array[0].TargetShape;

            for (int i = 1; i < array.Length; i++)
            {
                Data D = array[i];

                if (D.InputShape != InputShape || D.TargetShape != TargetShape)
                {
                    throw new ShapeMismatchException($"{nameof(array)} [{i}] mismatch.");
                }
            }
        }

        public Dataset(params Data[] array) : this(array, false)
        {
        }

        public new static Dataset FromRef(params Data[] array)
        {
            return new Dataset(array, true);
        }

        public void Normalize(INormalizer normalizer, bool inputs, bool targets)
        {
            if (!inputs && !targets)
            {
                return;
            }

            normalizer.UpdateParams(this, inputs, targets);

            for (int i = 0; i < Length; i++)
            {
                Data D = internalArray[i];

                internalArray[i] = new Data
                (
                    inputs ? D.Inputs.Select(X => normalizer.Normalize(X)).ToShape(InputShape) : D.Inputs,
                    targets ? D.Targets.Select(X => normalizer.Normalize(X)).ToShape(TargetShape) : D.Targets
                );
            }
        }

        public void Shuffle()
        {
            for (int i = 0; i < Length; i++)
            {
                int iRnd = Utils.GRandom.Next(i, Length);
                Data temp = internalArray[i];
                internalArray[i] = internalArray[iRnd];
                internalArray[iRnd] = temp;
            }
        }
    }
}
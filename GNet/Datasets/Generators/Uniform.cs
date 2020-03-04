﻿using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class Uniform : IDatasetGenerator
    {
        public int IOLength { get; }

        public Uniform(int ioLength)
        {
            IOLength = ioLength;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double[] io = new double[IOLength];

                for (int j = 0; j < IOLength; j++)
                {
                    io[j] = GRandom.NextDouble() < 0.5 ? 0.0 : 1.0;
                }

                dataCollection[i] = new Data(
                    new ShapedArrayImmutable<double>(new Shape(io.Length), io),
                    new ShapedArrayImmutable<double>(new Shape(io.Length), io));
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone()
        {
            return new Uniform(IOLength);
        }
    }
}
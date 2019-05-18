﻿using System;
using static System.Math;
using GNet.GlobalRandom;

namespace GNet
{
    public interface IInitializer : ICloneable
    {
        double Init(int nIn, int nOut);
    }
}

namespace GNet.Initializers
{    
    public class Nan : IInitializer
    {
        public double Init(int nIn, int nOut) => double.NaN;

        public object Clone() => new Nan();
    }

    public class Zero : IInitializer
    {
        public double Init(int nIn, int nOut) => 0;

        public object Clone() => new Zero();
    }

    public class Const : IInitializer
    {
        double value;

        public Const(double value)
        {
            this.value = value;
        }

        public double Init(int nIn, int nOut) => value;

        public object Clone() => new Const(value);
    }

    public class One : IInitializer
    {
        public double Init(int nIn, int nOut) => 1;

        public object Clone() => new One();
    }

    public class Uniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(-1, 1);

        public object Clone() => new Uniform();
    }

    public class Normal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian();

        public object Clone() => new Normal();
    }

    public class LeCunNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian() * Sqrt(1.0 / nIn);

        public object Clone() => new LeCunNormal();
    }

    public class HeNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian() * Sqrt(2.0 / nIn);

        public object Clone() => new HeNormal();
    }

    public class GlorotNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian() * Sqrt(2.0 / (nIn + nOut));

        public object Clone() => new GlorotNormal();
    }

    public class LeCunUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(3.0 / nIn));

        public object Clone() => new LeCunUniform();
    }

    public class HeUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(6.0 / nIn));
        public object Clone() => new HeUniform();
    }

    public class GlorotUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(6.0 / (nIn + nOut)));

        public object Clone() => new GlorotUniform();
    }
    
}

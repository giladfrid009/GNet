using GNet.GlobalRandom;
using System;
using static System.Math;

namespace GNet
{
    public interface IInitializer : ICloneable<IInitializer>
    {
        double Init(int nIn, int nOut);
    }
}

namespace GNet.Initializers
{
    [Serializable]
    public class Zero : IInitializer
    {
        public double Init(int nIn, int nOut) => 0.0;

        public IInitializer Clone() => new Zero();
    }

    [Serializable]
    public class Const : IInitializer
    {
        public double Value { get; }

        public Const(double value)
        {
            Value = value;
        }

        public double Init(int nIn, int nOut) => Value;

        public IInitializer Clone() => new Const(Value);
    }

    [Serializable]
    public class One : IInitializer
    {
        public double Init(int nIn, int nOut) => 1.0;

        public IInitializer Clone() => new One();
    }

    [Serializable]
    public class Uniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(-1.0, 1.0);

        public IInitializer Clone() => new Uniform();
    }

    [Serializable]
    public class Normal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextNormal();

        public IInitializer Clone() => new Normal();
    }

    [Serializable]
    public class LeCunNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextNormal() * Sqrt(1.0 / nIn);

        public IInitializer Clone() => new LeCunNormal();
    }

    [Serializable]
    public class HeNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextNormal() * Sqrt(2.0 / nIn);

        public IInitializer Clone() => new HeNormal();
    }

    [Serializable]
    public class GlorotNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextNormal() * Sqrt(2.0 / (nIn + nOut));

        public IInitializer Clone() => new GlorotNormal();
    }

    [Serializable]
    public class LeCunUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(3.0 / nIn));

        public IInitializer Clone() => new LeCunUniform();
    }

    [Serializable]
    public class HeUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(6.0 / nIn));

        public IInitializer Clone() => new HeUniform();
    }

    [Serializable]
    public class GlorotUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(6.0 / (nIn + nOut)));

        public IInitializer Clone() => new GlorotUniform();
    }
}

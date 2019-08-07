using GNet.GlobalRandom;
using System;
using static System.Math;

namespace GNet
{
    public interface IInitializer : ICloneable<IInitializer>
    {
        double Initialize(int nIn, int nOut);
    }
}

namespace GNet.Initializers
{
    [Serializable]
    public class Zero : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return 0.0;
        }

        public IInitializer Clone()
        {
            return new Zero();
        }
    }

    [Serializable]
    public class Const : IInitializer
    {
        public double Value { get; }

        public Const(double value)
        {
            Value = value;
        }

        public double Initialize(int nIn, int nOut)
        {
            return Value;
        }

        public IInitializer Clone()
        {
            return new Const(Value);
        }
    }

    [Serializable]
    public class One : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return 1.0;
        }

        public IInitializer Clone()
        {
            return new One();
        }
    }

    [Serializable]
    public class Uniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextDouble(-1.0, 1.0);
        }

        public IInitializer Clone()
        {
            return new Uniform();
        }
    }

    [Serializable]
    public class Normal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal();
        }

        public IInitializer Clone()
        {
            return new Normal();
        }
    }

    [Serializable]
    public class LeCunNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal() * Sqrt(1.0 / nIn);
        }

        public IInitializer Clone()
        {
            return new LeCunNormal();
        }
    }

    [Serializable]
    public class HeNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal() * Sqrt(2.0 / nIn);
        }

        public IInitializer Clone()
        {
            return new HeNormal();
        }
    }

    [Serializable]
    public class GlorotNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal() * Sqrt(2.0 / (nIn + nOut));
        }

        public IInitializer Clone()
        {
            return new GlorotNormal();
        }
    }

    [Serializable]
    public class LeCunUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextDouble(Sqrt(3.0 / nIn));
        }

        public IInitializer Clone()
        {
            return new LeCunUniform();
        }
    }

    [Serializable]
    public class HeUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextDouble(Sqrt(6.0 / nIn));
        }

        public IInitializer Clone()
        {
            return new HeUniform();
        }
    }

    [Serializable]
    public class GlorotUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextDouble(Sqrt(6.0 / (nIn + nOut)));
        }

        public IInitializer Clone()
        {
            return new GlorotUniform();
        }
    }
}

using GNet.GlobalRandom;
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
    public class Nan : IInitializer
    {
        public double Init(int nIn, int nOut) => double.NaN;

        public IInitializer Clone() => new Nan();
    }

    public class Zero : IInitializer
    {
        public double Init(int nIn, int nOut) => 0;

        public IInitializer Clone() => new Zero();
    }

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

    public class One : IInitializer
    {
        public double Init(int nIn, int nOut) => 1;

        public IInitializer Clone() => new One();
    }

    public class Uniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(-1, 1);

        public IInitializer Clone() => new Uniform();
    }

    public class Normal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian();

        public IInitializer Clone() => new Normal();
    }

    public class LeCunNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian() * Sqrt(1.0 / nIn);

        public IInitializer Clone() => new LeCunNormal();
    }

    public class HeNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian() * Sqrt(2.0 / nIn);

        public IInitializer Clone() => new HeNormal();
    }

    public class GlorotNormal : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextGaussian() * Sqrt(2.0 / (nIn + nOut));

        public IInitializer Clone() => new GlorotNormal();
    }

    public class LeCunUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(3.0 / nIn));

        public IInitializer Clone() => new LeCunUniform();
    }

    public class HeUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(6.0 / nIn));

        public IInitializer Clone() => new HeUniform();
    }

    public class GlorotUniform : IInitializer
    {
        public double Init(int nIn, int nOut) => GRandom.NextDouble(Sqrt(6.0 / (nIn + nOut)));

        public IInitializer Clone() => new GlorotUniform();
    }
}

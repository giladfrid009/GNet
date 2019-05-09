using System;
using static System.Math;
using GNet.Extensions;

namespace GNet
{
    public delegate double InitFunc(int nIn, int nOut);

    public enum Initializers { Zero, SmallConst, One, Uniform, Gaussian, LeCunNormal, HeNormal, XavierNormal, LeCunUniform, HeUniform, XavierUniform };

    static class InitializerProvider
    {
        static Random rnd = new Random();

        public static InitFunc GetInitializer(Initializers initializer)
        {
            switch (initializer)
            {
                case Initializers.Zero: return (nIn, nOut) => 0;

                case Initializers.SmallConst: return (nIn, nOut) => 0.01;

                case Initializers.One: return (nIn, nOut) => 1;

                case Initializers.Uniform: return (nIn, nOut) => rnd.NextDouble(-1, 1);

                case Initializers.Gaussian: return (nIn, nOut) => rnd.NextGaussian();

                case Initializers.LeCunNormal: return (nIn, nOut) => rnd.NextGaussian() * Sqrt(2 / nIn);

                case Initializers.HeNormal: return (nIn, nOut) => rnd.NextGaussian() * Sqrt(2 / nIn);

                case Initializers.XavierNormal: return (nIn, nOut) => rnd.NextGaussian() * Sqrt(2 / (nIn + nOut));

                case Initializers.LeCunUniform:
                {
                    return (nIn, nOut) =>
                    {
                        var bound = Sqrt(3 / nIn);
                        return rnd.NextDouble(-bound, bound);
                    };
                }

                case Initializers.HeUniform:
                {
                    return (nIn, nOut) =>
                    {
                        var bound = Sqrt(6 / nIn);
                        return rnd.NextDouble(-bound, bound);
                    };
                }

                case Initializers.XavierUniform:
                {
                    return (nIn, nOut) =>
                    {
                        var bound = Sqrt(6 / (nIn + nOut));
                        return rnd.NextDouble(-bound, bound);
                    };
                }

                default: throw new ArgumentOutOfRangeException("Unknown initializer");
            }
        }
    }
}

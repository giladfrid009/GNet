using GNet.Extensions;
using System;
using static System.Math;

namespace GNet
{
    public delegate double InitFunc(int nIn, int nOut);

    public enum Initializers { Zero, SmallConst, One, Uniform, Gaussian, LeCunNormal, HeNormal, XavierNormal, LeCunUniform, HeUniform, XavierUniform };

    internal static class InitializerProvider
    {
        private static readonly Random rnd = new Random();

        public static InitFunc GetInitializer(Initializers initializer)
        {
            switch (initializer)
            {
                case Initializers.Zero: return (nIn, nOut) => 0;

                case Initializers.SmallConst: return (nIn, nOut) => 0.01;

                case Initializers.One: return (nIn, nOut) => 1;

                case Initializers.Uniform: return (nIn, nOut) => rnd.NextDouble();

                case Initializers.Gaussian: return (nIn, nOut) => rnd.NextGaussian();

                case Initializers.LeCunNormal: return (nIn, nOut) => rnd.NextGaussian() * Sqrt(1 / nIn);

                case Initializers.HeNormal: return (nIn, nOut) => rnd.NextGaussian() * Sqrt(2 / nIn);

                case Initializers.XavierNormal: return (nIn, nOut) => rnd.NextGaussian() * Sqrt(2 / (nIn + nOut));

                case Initializers.LeCunUniform: return (nIn, nOut) => rnd.NextDouble(Sqrt(3 / nIn));

                case Initializers.HeUniform: return (nIn, nOut) => rnd.NextDouble(Sqrt(6 / nIn));

                case Initializers.XavierUniform: return (nIn, nOut) => rnd.NextDouble(Sqrt(6 / (nIn + nOut)));

                default: throw new ArgumentOutOfRangeException("Unknown initializer");
            }
        }
    }
}

using GNet.Extensions;
using System;
using static System.Math;

namespace GNet
{
    /// <summary>
    /// Value initialization function.
    /// </summary>
    /// <param name="nIn">Weight's input layer length.</param>
    /// <param name="nOut">Weight's output layer length.</param>
    public delegate double InitFunc(int nIn, int nOut);

    public enum Initializers { Zero, SmallConst, One, Uniform, Normal, LeCunNormal, HeNormal, XavierNormal, LeCunUniform, HeUniform, XavierUniform };

    internal static class InitializerProvider
    {
        /// <summary>
        /// Returns an initialization function.
        /// </summary>
        public static InitFunc GetInitializer(Initializers initializer)
        {
            switch (initializer)
            {
                case Initializers.Zero: return (nIn, nOut) => 0.0;

                case Initializers.SmallConst: return (nIn, nOut) => 0.01;

                case Initializers.One: return (nIn, nOut) => 1.0;

                case Initializers.Uniform: return (nIn, nOut) => GRandom.Rnd.NextDouble(-1, 1);

                case Initializers.Normal: return (nIn, nOut) => GRandom.Rnd.NextGaussian();

                case Initializers.LeCunNormal: return (nIn, nOut) => GRandom.Rnd.NextGaussian() * Sqrt(1.0 / nIn);

                case Initializers.HeNormal: return (nIn, nOut) => GRandom.Rnd.NextGaussian() * Sqrt(2.0 / nIn);

                case Initializers.XavierNormal: return (nIn, nOut) => GRandom.Rnd.NextGaussian() * Sqrt(2.0 / (nIn + nOut));

                case Initializers.LeCunUniform: return (nIn, nOut) => GRandom.Rnd.NextDouble(Sqrt(3.0 / nIn));

                case Initializers.HeUniform: return (nIn, nOut) => GRandom.Rnd.NextDouble(Sqrt(6.0 / nIn));

                case Initializers.XavierUniform: return (nIn, nOut) => GRandom.Rnd.NextDouble(Sqrt(6.0 / (nIn + nOut)));

                default: throw new ArgumentOutOfRangeException("Unknown initializer");
            }
        }
    }
}

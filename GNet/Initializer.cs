using System;
using static System.Math;
using GNet.Extensions;

namespace GNet
{
    public enum Initializers { Zero, SmallConst, One, Uniform, Gaussian, He, Xavier };

    static class Initializer
    {
        static Random rnd = new Random();

        public static double GenerateValue(Initializers initializer, int? prevLayerLength = null)
        {
            switch (initializer)
            {
                case Initializers.Zero: return 0;

                case Initializers.SmallConst: return 0.01;

                case Initializers.One: return 1;

                case Initializers.Uniform: return rnd.NextDouble(-1, 1);

                case Initializers.Gaussian: return rnd.NextGaussian();

                case Initializers.He:
                {
                    if (prevLayerLength.HasValue) return rnd.NextGaussian() * Sqrt(2 / prevLayerLength.Value);

                    throw new ArgumentNullException("prevLayerLength must be specified for this initialization");
                }

                case Initializers.Xavier:
                {
                    if (prevLayerLength.HasValue) return rnd.NextGaussian() * Sqrt(2 / prevLayerLength.Value);

                    throw new ArgumentNullException("prevLayerLength must be specified for this initialization");
                }

                default: throw new ArgumentOutOfRangeException("Unknown initializer");
            }
        }
    }
}

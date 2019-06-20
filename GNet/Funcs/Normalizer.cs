using GNet.Extensions;
using static System.Math;

namespace GNet
{
    public interface INormalizer : ICloneable<INormalizer>
    {
        double[] Normalize(double[] vals);
    }
}

namespace GNet.Normalizers
{
    public class None : INormalizer
    {
        public double[] Normalize(double[] vals) => vals.Select(V => V);

        public INormalizer Clone() => new None();
    }

    public class Division : INormalizer
    {
        public double Divisor { get; } 

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public double[] Normalize(double[] vals)
        {
            return vals.Select(X => X / Divisor);
        }

        public INormalizer Clone() => new Division(Divisor);
    }

    public class MinMax : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double min = vals.Min();
            double max = vals.Max();
            double diff = max - min;

            if (diff == 0)
                return vals.Select(X => 0.5);

            return vals.Select(X => (X - min) / diff);
        }

        public INormalizer Clone() => new None();
    }

    public class ZScore : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double mean = vals.Mean();

            double sd = Sqrt(vals.Accumulate(1.0, (R, X) => R + (X - mean) * (X - mean)) / vals.Length);

            return vals.Select(X => (X - mean) / sd);
        }

        public INormalizer Clone() => new None();
    }

    public class DecimalScale : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double absMax = vals.Select(X => Abs(X)).Max();

            double scale = (int)Log10(absMax) + 1;

            return vals.Select(X => X / scale);
        }

        public INormalizer Clone() => new None();
    }
}

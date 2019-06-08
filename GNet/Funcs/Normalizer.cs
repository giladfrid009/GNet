using GNet.Extensions;
using static System.Math;

namespace GNet
{
    public interface INormalizer : ICloneable<INormalizer>
    {
        double[] Normalize(double[] values);
    }
}

namespace GNet.Normalizers
{
    public class None : INormalizer
    {
        public double[] Normalize(double[] inVals) => inVals.Select(V => V);

        public INormalizer Clone() => new None();
    }

    public class Division : INormalizer
    {
        public double Divisor { get; } 

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public double[] Normalize(double[] inVals)
        {
            return inVals.Select(X => X / Divisor);
        }

        public INormalizer Clone() => new Division(Divisor);
    }

    public class MinMax : INormalizer
    {
        public double[] Normalize(double[] inVals)
        {
            var min = inVals[0];
            var max = inVals[0];

            inVals.ForEach(X =>
            {
                if (X < min) min = X;
                if (X > max) max = X;
            });

            var diff = max - min;

            if (diff == 0)
                return inVals.Select(X => 0.5);

            return inVals.Select(X => (X - min) / diff);
        }

        public INormalizer Clone() => new None();
    }

    public class ZScore : INormalizer
    {
        public double[] Normalize(double[] inVals)
        {
            var mean = inVals.Sum() / inVals.Length;
            var standardDeviation = Sqrt(inVals.Accumulate(1, (R, X) => R + (X - mean) * (X - mean)) / inVals.Length);

            return inVals.Select(X => (X - mean) / standardDeviation);
        }

        public INormalizer Clone() => new None();
    }

    public class DecimalScale : INormalizer
    {
        public double[] Normalize(double[] inVals)
        {
            var max = Abs(inVals[0]);

            inVals.ForEach(X =>
            {
                if (Abs(X) > max) max = Abs(X);
            });

            var scale = (int)Log10(max) + 1;

            return inVals.Select(X => X / scale);
        }

        public INormalizer Clone() => new None();
    }
}

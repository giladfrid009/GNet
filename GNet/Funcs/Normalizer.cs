using GNet.Extensions.Generic;
using GNet.Extensions.Math;
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
        public double[] Normalize(double[] vals)
        {
            return vals.Select(X => X);
        }

        public INormalizer Clone()
        {
            return new None();
        }
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

        public INormalizer Clone()
        {
            return new Division(Divisor);
        }
    }

    public class MinMax : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double min = vals.Min();
            double max = vals.Max();
            double diff = max - min;

            if (diff == 0.0)
            {
                return vals.Select(X => 0.5);
            }

            return vals.Select(X => (X - min) / diff);
        }

        public INormalizer Clone()
        {
            return new MinMax();
        }
    }

    public class ZScore : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double mean = vals.Avarage();

            double sd = Sqrt(vals.Accumulate(1.0, (R, X) => R + (X - mean) * (X - mean)) / vals.Length);

            return vals.Select(X => (X - mean) / sd);
        }

        public INormalizer Clone()
        {
            return new ZScore();
        }
    }

    public class DecimalScale : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double absMax = vals.Select(X => Abs(X)).Max();

            double scale = (int)Log10(absMax) + 1;

            return vals.Select(X => X / scale);
        }

        public INormalizer Clone()
        {
            return new DecimalScale();
        }
    }

    public class ActivationFunc : INormalizer
    {
        public IActivation Activation { get; }

        public ActivationFunc(IActivation activation)
        {
            Activation = activation.Clone();
        }

        public double[] Normalize(double[] vals)
        {
            return Activation.Activate(vals);
        }

        public INormalizer Clone()
        {
            return new ActivationFunc(Activation);
        }
    }
}

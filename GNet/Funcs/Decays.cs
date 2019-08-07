namespace GNet
{
    public interface IDecay : ICloneable<IDecay>
    {
        double Decay { get; }

        double Compute(double value, int iteration);
    }
}

namespace GNet.Optimizers.Decays
{
    using static System.Math;

    public class None : IDecay
    {
        public double Decay { get; } = 0;

        public double Compute(double value, int iteration)
        {
            return value;
        }

        public IDecay Clone()
        {
            return new None();
        }
    }

    public class Subtraction : IDecay
    {
        public double Decay { get; }
        public int Interval { get; }

        public Subtraction(double decay, int interval)
        {
            Decay = decay;
            Interval = interval;
        }

        public double Compute(double value, int iteration)
        {
            return value - Decay * (iteration / Interval);
        }

        public IDecay Clone()
        {
            return new Subtraction(Decay, Interval);
        }
    }

    public class Multiplication : IDecay
    {
        public double Decay { get; }
        public double Multiplier { get; }
        public int Interval { get; }

        public Multiplication(double decay, int interval)
        {
            Decay = decay;
            Interval = interval;
            Multiplier = 1.0 - Decay;
        }

        public double Compute(double value, int iteration)
        {
            return value * Pow(Multiplier, iteration / Interval);
        }

        public IDecay Clone()
        {
            return new Multiplication(Decay, Interval);
        }
    }

    public class Exponential : IDecay
    {
        public double Decay { get; }

        public Exponential(double decay)
        {
            Decay = decay;
        }

        public double Compute(double value, int iteration)
        {
            return value * Exp(-Decay * iteration);
        }

        public IDecay Clone()
        {
            return new Exponential(Decay);
        }
    }

    public class IterationBased : IDecay
    {
        public double Decay { get; }

        public IterationBased(double decay)
        {
            Decay = decay;
        }

        public double Compute(double value, int iteration)
        {
            return value / (1.0 + Decay * iteration);
        }

        public IDecay Clone()
        {
            return new IterationBased(Decay);
        }
    }
}

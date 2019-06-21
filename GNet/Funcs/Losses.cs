using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet
{
    public interface ILoss : ICloneable<ILoss>
    {
        double Compute(double[] targets, double[] outputs);

        double[] Derivative(double[] targets, double[] outputs);
    }
}

namespace GNet.Losses
{
    /// <summary>
    /// Mean Squared Error
    /// </summary>
    public class MSE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) * (T - O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => 2.0 * (O - T));
        }

        public ILoss Clone() => new MSE();
    }

    /// <summary>
    /// Mean Squared Logarithmic Error
    /// </summary>
    public class MSLE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => (Log(T + 1.0) - Log(O + 1.0)) * (Log(T + 1.0) - Log(O + 1.0))).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -2.0 * (Log(T + 1.0) - Log(O + 1.0)) / (O + 1.0));
        }

        public ILoss Clone() => new MSLE();
    }

    /// <summary>
    /// Mean Absolute Error
    /// </summary>
    public class MAE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T > O ? 1.0 : -1.0);
        }

        public ILoss Clone() => new MAE();
    }

    /// <summary>
    /// Mean Absolute Percentage Error
    /// </summary>
    public class MAPE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs((T - O) / T)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => O * (T - O) / (Pow(T, 3.0) * Abs(1.0 - O / T)));
        }

        public ILoss Clone() => new MAPE();
    }

    public class LogCosh : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Log(Cosh(T - O))).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Tanh(O - T));
        }

        public ILoss Clone() => new LogCosh();
    }

    public class KLDivergence : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T * Log(T / O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / O);
        }

        public ILoss Clone() => new KLDivergence();
    }

    public class BinaryCrossEntropy : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O) - (1.0 - T) * Log(1.0 - O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) / (O * O - O));
        }

        public ILoss Clone() => new BinaryCrossEntropy();
    }

    public class CategoricalCrossEntropy : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / O);
        }

        public ILoss Clone() => new CategoricalCrossEntropy();
    }

    public class NegativeLogLiklihood : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -Log(O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -1.0 / O);
        }

        public ILoss Clone() => new NegativeLogLiklihood();
    }

    public class Poisson : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => O - T * Log(O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => 1.0 - (T / O));
        }

        public ILoss Clone() => new Poisson();
    }

    public class CosineProximity : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Accumulate(1.0, (R, X) => R + X * X);
            double oSumSqr = outputs.Accumulate(1.0, (R, X) => R + X * X);

            return -tProd * oProd / (Sqrt(tSumSqr) * Sqrt(oSumSqr));
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Accumulate(1.0, (R, X) => R + X * X);
            double oSumSqr = outputs.Accumulate(1.0, (R, X) => R + X * X);

            return outputs.Select(O => -tProd * (oProd / O) * Pow(oSumSqr - O * O, 2.0) / (Abs(tSumSqr) * Pow(oSumSqr, 1.5)));
        }

        public ILoss Clone() => new CosineProximity();
    }

    public class Hinge : ILoss
    {
        public double Margin { get; }

        public Hinge(double margin = 1.0)
        {
            Margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Max(0, Margin - T * O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T * O < Margin ? -T : 0.0);
        }

        public ILoss Clone() => new Hinge(Margin);
    }

    public class HingeSquared : ILoss
    {
        public double Margin { get; }

        public HingeSquared(double margin = 1.0)
        {
            Margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Max(0.0, Margin - T * O) * Max(0.0, Margin - T * O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T * O < Margin ? -2.0 * T * (Margin - T * O) : 0.0);
        }

        public ILoss Clone() => new HingeSquared(Margin);
    }

    public class Huber : ILoss
    {
        public double Margin { get; }

        public Huber(double margin)
        {
            Margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) =>
            {
                double diff = Abs(T - O);

                if (diff <= Margin)
                    return O = 0.5 * diff * diff;

                else
                    return O = Margin * (diff - 0.5 * Margin);
            })
            .Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O) <= Margin ? O - T : -Margin);
        }

        public ILoss Clone() => new Huber(Margin);
    }
}

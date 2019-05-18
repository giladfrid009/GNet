using GNet.Extensions;
using System;
using static System.Math;

namespace GNet
{
    public interface ILoss
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
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => Abs(T - O) * Abs(T - O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => 2 * (O - T));
        }
    }

    /// <summary>
    /// Mean Squared Logarithmic Error
    /// </summary>
    public class MSLE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => (Log(T + 1) - Log(O + 1)) * (Log(T + 1) - Log(O + 1))).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -2 * (Log(T + 1) - Log(O + 1)) / (O + 1));
        }
    }

    /// <summary>
    /// Mean Absolute Error
    /// </summary>
    public class MAE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => Abs(T - O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T > O ? 1 : -1);
        }
    }

    /// <summary>
    /// Mean Absolute Percentage Error
    /// </summary>
    public class MAPE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => Abs((T - O) / T)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => O * (T - O) / (Pow(T, 3) * Abs(1 - O / T)));
        }
    }

    public class LogCosh : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => Log(Cosh(T - O))).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Tanh(O - T));
        }
    }

    public class KLDivergence : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => T * Log(T / O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / O);
        }
    }

    public class BinaryCrossEntropy : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => -T * Log(O) - (1 - T) * Log(1 - O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) / (O * O - O));
        }
    }

    public class CategoricalCrossEntropy : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => -T * Log(O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / O);
        }
    }

    public class NegativeLogLiklihood : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => -Log(O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -1 / O);
        }
    }

    public class Poisson : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => O - T * Log(O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => 1 - (T / O));
        }
    }

    public class CosineProximity : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            double tProd = targets.Accumulate(1, (R, X) => R * X);
            double oProd = outputs.Accumulate(1, (R, X) => R * X);
            double tSumSqr = targets.Accumulate(1, (R, X) => R + X * X);
            double oSumSqr = outputs.Accumulate(1, (R, X) => R + X * X);

            return -1 * tProd * oProd / (Sqrt(tSumSqr) * Sqrt(oSumSqr));
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            double tProd = targets.Accumulate(1, (R, X) => R * X);
            double oProd = outputs.Accumulate(1, (R, X) => R * X);
            double tSumSqr = targets.Accumulate(1, (R, X) => R + X * X);
            double oSumSqr = outputs.Accumulate(1, (R, X) => R + X * X);

            return outputs.Map(O => tProd * (oProd - O) * (oSumSqr - O * O) / (Sqrt(tSumSqr) * Pow(oSumSqr, 1.5)));
        }
    }

    public class Hinge : ILoss
    {
        private readonly double margin;

        public Hinge(double margin = 1)
        {
            this.margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => Max(0, margin - T * O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T * O < margin ? -T : 0);
        }
    }

    public class HingeSquared : ILoss
    {
        private readonly double margin;

        public HingeSquared(double margin = 1)
        {
            this.margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) => Max(0, margin - T * O) * Max(0, margin - T * O)).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T * O < margin ? -2 * T * (margin - T * O) : 0);
        }
    }

    public class Huber : ILoss
    {
        private readonly double margin;

        public Huber(double margin)
        {
            this.margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            int N = Min(targets.Length, outputs.Length);

            return targets.Combine(outputs, (T, O) =>
            {
                double diff = Abs(T - O);

                if (diff <= margin)
                    return O = 0.5 * diff * diff;

                else
                    return O = margin * (diff - 0.5 * margin);
            }).Sum() / N;
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O) <= margin ? O - T : -margin);
        }        
    }
}

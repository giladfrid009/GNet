using System;
using static System.Math;

namespace GNet.LossFuncs
{
    // todo: implement
    // todo: add more losses
    // todo: error check
    public interface ILoss
    {
        double Compute(double[] outputs, double[] targets);

        double[] Derivative(double[] outputs, double[] targets);
    }

    /// <summary>
    /// Mean Squared Error
    /// </summary>
    public class MSE : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                var diff = Abs(targets[i] - outputs[i]);
                totalLoss += diff * diff;
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = 2 * (outputs[i] - targets[i]);
            }

            return derived;
        }
    }

    /// <summary>
    /// Mean Squared Logarithmic Error
    /// </summary>
    public class MSLE : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                var diff = Log(targets[i] + 1) - Log(outputs[i] + 1);
                totalLoss += diff * diff;
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = -2 * (Log(targets[i] + 1) - Log(outputs[i] + 1)) / (outputs[i] + 1);
            }

            return derived;
        }
    }

    /// <summary>
    /// Mean Absolute Error
    /// </summary>
    public class MAE : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += Abs(targets[i] - outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = targets[i] > outputs[i] ? 1 : -1;
            }

            return derived;
        }

    }

    /// <summary>
    /// Mean Absolute Percentage Error
    /// </summary>
    public class MAPE : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += Abs((targets[i] - outputs[i]) / targets[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = outputs[i] * (targets[i] - outputs[i]) / (Pow(targets[i], 3) * Abs(1 - outputs[i] / targets[i]));
            }

            return derived;
        }
    }

    public class LogCosh : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += Log(Cosh(targets[i] - outputs[i]));
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = Tanh(outputs[i] - targets[i]);
            }

            return derived;
        }
    }

    /// <summary>
    /// Kullback Leibler Divergence
    /// </summary>
    public class KLDivergence : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += targets[i] * Log(targets[i] / outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = -targets[i] / outputs[i];
            }

            return derived;
        }
    }

    public class BinaryCrossEntropy : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += -targets[i] * Log(outputs[i]) - (1 - targets[i]) * Log(1 - outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = (targets[i] - outputs[i]) / (outputs[i] * outputs[i] - outputs[i]);
            }

            return derived;
        }
    }

    public class CategoricalCrossEntropy : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += -targets[i] * Log(outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = -targets[i] / outputs[i];
            }

            return derived;
        }
    }

    public class NegativeLogLiklihood : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += -Log(outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = -1 / outputs[i];
            }

            return derived;
        }
    }

    public class Poisson : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += outputs[i] - targets[i] * Log(outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = 1 - (targets[i] / outputs[i]);
            }

            return derived;
        }
    }

    public class CosineProximity : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double sumO = 0;
            double sumT = 0;
            double sumBoth = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                sumO += outputs[i] * outputs[i];
                sumT += targets[i] * targets[i];
                sumBoth += outputs[i] * targets[i];
            }

            double cosP = -sumBoth / Sqrt(sumO * sumT);

            return cosP;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            double sumO = 0;
            double sumT = 0;
            double sumBoth = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                sumO += outputs[i] * outputs[i];
                sumT += targets[i] * targets[i];
                sumBoth += outputs[i] * targets[i];
            }

            double sqrO = Sqrt(sumO);
            double sqrT = Sqrt(sumT);
            double cosP = -sumBoth / (sqrO * sqrT);

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = cosP + outputs[i] * (sumT / (sqrT * sqrO) - cosP * sumO / Pow(sqrO, 2));
            }

            return derived;
        }
    }

    public class Hinge : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += Max(0, 1 - targets[i] * outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = -targets[i] / outputs[i];
            }

            return derived;
        }
    }

    /// <summary>
    /// Parametric Hinge
    /// </summary>
    public class PHinge : ILoss
    {
        private readonly double alpha;

        public PHinge(double alpha)
        {
            this.alpha = alpha;
        }

        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += Max(0, alpha - targets[i] * outputs[i]);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = -targets[i] / outputs[i];
            }

            return derived;
        }
    }

    public class HingeSquared : ILoss
    {
        public double Compute(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double totalLoss = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                totalLoss += Pow(Max(0, 1 - targets[i] * outputs[i]), 2);
            }

            return totalLoss / outputs.Length;
        }

        public double[] Derivative(double[] outputs, double[] targets)
        {
            if (outputs.Length != targets.Length)
                throw new ArgumentException("outputs and targets lengths mismatch");

            double[] derived = new double[outputs.Length];

            for (int i = 0; i < outputs.Length; i++)
            {
                derived[i] = targets[i] * outputs[i] < 1 ? 2 * targets[i] * (targets[i] * outputs[i] - 1) : 0;
            }

            return derived;
        }
    }
}

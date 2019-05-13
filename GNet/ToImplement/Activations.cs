using System;
using static System.Math;

namespace GNet.ActivationFuncs
{
    // TODO: IMPLEMENT THIS FILE INSTEAD OF LossProvider!
    // todo: add more activations, remove useless activations
    // todo: check for errors in calculations, etc.

    // https://en.wikipedia.org/wiki/Activation_function

    public interface IActivation
    {
        double[] Activate(double[] vals);

        double[] Derivative(double[] vals);
    }

    public class Identity : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 1.0;
            }

            return derived;
        }
    }

    public class BinaryStep : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] < 0 ? 0 : 1;
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 0;
            }

            return derived;
        }
    }

    public class Sigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = 1 / (1 + Exp(-vals[i]));
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = Exp(vals[i]) / Pow(1 + Exp(-vals[i]), 2);
            }

            return derived;
        }
    }

    public class HardSigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] < -2.5 ? 0 : vals[i] > 2.5 ? 1 : 0.2 * vals[i] + 0.5;
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] < -2.5 || vals[i] > 2.5 ? 0 : 0.2;
            }

            return derived;
        }
    }

    public class Tanh : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = Tanh(vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 1 / Pow(Cosh(vals[i]), 2);
            }

            return derived;
        }
    }

    public class LeCunTanh : IActivation
    {
        private readonly double alpha = 1.7159;
        private readonly double beta = 2.0 / 3;

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = alpha * Tanh(beta * vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = alpha * beta / Pow(Cosh(beta * vals[i]), 2);
            }

            return derived;
        }
    }

    public class ArcTan : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = Atan(vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 1 / (1 + vals[i] * vals[i]);
            }

            return derived;
        }
    }

    public class ArSinh : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = Asinh(vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 1 / Sqrt(1 + vals[i] * vals[i]);
            }

            return derived;
        }
    }

    public class SoftSign : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] / (1 + Abs(vals[i]));
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 1 / Pow(1 + Abs(vals[i]), 2);
            }

            return derived;
        }
    }

    /// <summary>
    /// Inverse Square Root Unit
    /// </summary>
    public class ISRU : IActivation
    {
        private readonly double alpha;

        public ISRU(double alpha)
        {
            this.alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] / Sqrt(1 + alpha * vals[i] * vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = Pow(vals[i] / Sqrt(1 + alpha * vals[i] * vals[i]), 3);
            }

            return derived;
        }
    }

    /// <summary>
    /// Inverse Square Root Linear Unit
    /// </summary>
    public class ISRLU : IActivation
    {
        private readonly double alpha;

        public ISRLU(double alpha)
        {
            this.alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                if (vals[i] >= 0)
                {
                    activated[i] = vals[i];
                }
                else
                {
                    activated[i] = vals[i] / Sqrt(1 + alpha * vals[i] * vals[i]);
                }
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                if (vals[i] >= 0)
                {
                    derived[i] = 1;
                }
                else
                {
                    derived[i] = Pow(vals[i] / Sqrt(1 + alpha * vals[i] * vals[i]), 3);
                }
            }

            return derived;
        }
    }

    /// <summary>
    /// Square Nonlinearity 
    /// </summary>
    public class SQNL : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                if (vals[i] > 2)
                {
                    activated[i] = 1;
                }
                else if (vals[i] >= 0 && vals[i] <= 2)
                {
                    activated[i] = vals[i] - vals[i] * vals[i] / 4;
                }
                else if (vals[i] > 0 && vals[i] <= -2)
                {
                    activated[i] = vals[i] + vals[i] * vals[i] / 4;
                }
                else
                {
                    activated[i] = -1;
                }
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                if (vals[i] >= 0)
                {
                    derived[i] = 1 - vals[i] / 2;
                }
                else
                {
                    derived[i] = 1 + vals[i] / 2;
                }
            }

            return derived;
        }
    }

    /// <summary>
    /// Rectified Linear Unit
    /// </summary>
    public class ReLu : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] < 0 ? 0 : vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] < 0 ? 0 : 1;
            }

            return derived;
        }
    }

    /// <summary>
    /// Leaky Rectified Linear Unit
    /// </summary>
    public class LeakyReLu : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] < 0 ? 0.01 * vals[i] : vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] < 0 ? 0.01 : 1;
            }

            return derived;
        }
    }

    /// <summary>
    /// Parametric Rectified Linear Unit
    /// </summary>
    public class PReLu : IActivation
    {
        private readonly double alpha;

        public PReLu(double alpha)
        {
            this.alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] < 0 ? alpha * vals[i] : vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] < 0 ? alpha : 1;
            }

            return derived;
        }
    }

    /// <summary>
    /// Randomized Rectified Linear Unit
    /// </summary>
    public class RReLu : IActivation
    {
        private readonly double alpha;

        public RReLu()
        {
            alpha = GRandom.Rnd.NextDouble();
        }

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] < 0 ? alpha * vals[i] : vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] < 0 ? alpha : 1;
            }

            return derived;
        }
    }

    /// <summary>
    /// Exponential Linear Unit
    /// </summary>
    public class ELU : IActivation
    {
        private readonly double alpha;

        public ELU(double alpha)
        {
            this.alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] <= 0 ? alpha * (Exp(vals[i]) - 1) : vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] <= 0 ? alpha * Exp(vals[i]) : 1;
            }

            return derived;
        }
    }

    /// <summary>
    /// Scaled Exponential Linear Unit
    /// </summary>
    public class SELU : IActivation
    {
        private readonly double alpha = 1.6732632423543772;
        private readonly double scale = 1.0507009873554805;

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] <= 0 ? scale * alpha * (Exp(vals[i]) - 1) : scale * vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] <= 0 ? scale * alpha * Exp(vals[i]) : scale;
            }

            return derived;
        }
    }

    public class SoftPlus : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = Log(1 + Exp(vals[i]));
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 1 / (1 + Exp(-vals[i]));
            }

            return derived;
        }
    }

    public class BentIdentity : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = (Sqrt(vals[i] * vals[i] + 1) - 1) / 2 + vals[i];
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] / (2 * Sqrt(vals[i] * vals[i] + 1)) + 1;
            }

            return derived;
        }
    }

    public class Swish : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] / (Exp(-vals[i]) + 1);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                var ex = Exp(vals[i]);

                derived[i] = ex * (1 + ex + vals[i]) / Pow(1 + ex, 2);
            }

            return derived;
        }
    }

    public class SoftExponential : IActivation
    {
        private readonly Func<double, double> activation;
        private readonly Func<double, double> derivative;

        public SoftExponential(double alpha)
        {
            if (alpha < 0.0)
            {
                activation = (val) => -Log(1 - alpha * (val + alpha)) / alpha;
                derivative = (val) => 1 / (1 - alpha * (alpha + val));
            }
            else if (alpha == 0.0)
            {
                activation = (val) => val;
                derivative = (val) => 1;
            }
            else
            {
                activation = (val) => (Exp(alpha * val) - 1) / alpha + alpha;
                derivative = (val) => Exp(alpha * val);
            }
        }

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activation(vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derivative(vals[i]);
            }

            return derived;
        }
    }

    public class SoftClipping : IActivation
    {
        private readonly double alpha;

        public SoftClipping(double alpha)
        {
            this.alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = 1 / alpha * Log((1 + Exp(alpha * vals[i])) / (1 + Exp(alpha * vals[i] - alpha)));
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = 0.5 * Sinh(0.5 * alpha) * (1 / Cosh(0.5 * alpha * vals[i])) * (1 / Cosh(0.5 * alpha * (1 - vals[i])));
            }

            return derived;
        }
    }

    public class Sinusoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = Sin(vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = Cos(vals[i]);
            }

            return derived;
        }
    }

    public class Sinc : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = vals[i] != 0 ? Sin(vals[i]) / vals[i] : 1;
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = vals[i] != 0 ? Cos(vals[i]) / vals[i] - Sin(vals[i]) / (vals[i] * vals[i]) : 0;
            }

            return derived;
        }
    }

    public class Gaussian : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = Exp(-vals[i] * vals[i]);
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = -2 * vals[i] * Exp(-vals[i] * vals[i]);
            }

            return derived;
        }
    }

    public class Softmax : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] activated = new double[vals.Length];
            double[] exp = new double[vals.Length];
            double expSum = 0;

            for (int i = 0; i < activated.Length; i++)
            {
                exp[i] = Exp(vals[i]);
                expSum += exp[i];
            }

            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = exp[i] / expSum;
            }

            return activated;
        }

        public double[] Derivative(double[] vals)
        {
            double[] derived = new double[vals.Length];
            double[] exp = new double[vals.Length];
            double expSum = 0;

            for (int i = 0; i < derived.Length; i++)
            {
                exp[i] = Exp(vals[i]);
                expSum += exp[i];
            }

            for (int i = 0; i < derived.Length; i++)
            {
                derived[i] = (expSum - exp[i]) * exp[i] / (expSum * expSum);
            }

            return derived;
        }
    }
}

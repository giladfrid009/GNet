using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using GNet.GlobalRandom;
using System;
using static System.Math;

namespace GNet
{
    public interface IActivation : ICloneable<IActivation>
    {
        double[] Activate(double[] vals);

        double[] Derivative(double[] vals);
    }
}

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0);
        }

        public IActivation Clone()
        {
            return new Identity();
        }
    }

    [Serializable]
    public class BinaryStep : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X > 0.0 ? 1.0 : 0.0);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 0.0);
        }

        public IActivation Clone()
        {
            return new BinaryStep();
        }
    }

    [Serializable]
    public class Sigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => Exp(X) / Pow(Exp(X) + 1.0, 2.0));
        }

        public IActivation Clone()
        {
            return new Sigmoid();
        }
    }

    [Serializable]
    public class HardSigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < -2.5 ? 0.0 : X > 2.5 ? 1.0 : 0.2 * X + 0.5);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < -2.5 || X > 2.5 ? 0.0 : 0.2);
        }

        public IActivation Clone()
        {
            return new HardSigmoid();
        }
    }

    [Serializable]
    public class Tanh : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Math.Tanh(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / Pow(Cosh(X), 2));
        }

        public IActivation Clone()
        {
            return new Tanh();
        }
    }

    [Serializable]
    public class LeCunTanh : IActivation
    {
        public double A { get; } = 1.7159;
        public double B { get; } = 2.0 / 3.0;

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => A * Math.Tanh(B * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => A * B / Pow(Cosh(B * X), 2.0));
        }

        public IActivation Clone()
        {
            return new LeCunTanh();
        }
    }

    [Serializable]
    public class ArcTan : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Atan(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / (1.0 + X * X));
        }

        public IActivation Clone()
        {
            return new ArcTan();
        }
    }

    [Serializable]
    public class ArSinh : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Asinh(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / Sqrt(1.0 + X * X));
        }

        public IActivation Clone()
        {
            return new ArSinh();
        }
    }

    [Serializable]
    public class SoftSign : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X / (1.0 + Abs(X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / Pow(1.0 + Abs(X), 2));
        }

        public IActivation Clone()
        {
            return new SoftSign();
        }
    }

    [Serializable]
    /// <summary>
    /// Inverse Square Root Unit
    /// </summary>
    public class ISRU : IActivation
    {
        public double Alpha { get; }

        public ISRU(double alpha)
        {
            Alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X / Sqrt(1.0 + Alpha * X * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => Pow(X / Sqrt(1.0 + Alpha * X * X), 3.0));
        }

        public IActivation Clone()
        {
            return new ISRU(Alpha);
        }
    }

    [Serializable]
    /// <summary>
    /// Inverse Square Root Linear Unit
    /// </summary>
    public class ISRLU : IActivation
    {
        public double Alpha { get; }

        public ISRLU(double alpha)
        {
            Alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X >= 0.0 ? X : X / Sqrt(1.0 + Alpha * X * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X >= 0.0 ? 1.0 : Pow(X / Sqrt(1.0 + Alpha * X * X), 3.0));
        }

        public IActivation Clone()
        {
            return new ISRLU(Alpha);
        }
    }

    [Serializable]
    /// <summary>
    /// Square Nonlinearity 
    /// </summary>
    public class SQNL : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X =>
            {
                if (X > 2.0)
                {
                    return 1.0;
                }

                if (X >= 0.0 && X <= 2.0)
                {
                    return X - X * X / 4.0;
                }

                if (X >= -2.0 && X < 0.0)
                {
                    return X - X * X / 4.0;
                }

                return -1.0;
            });
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X > 0.0 ? 1.0 - X / 2.0 : 1.0 + X / 2.0);
        }

        public IActivation Clone()
        {
            return new SQNL();
        }
    }

    [Serializable]
    /// <summary>
    /// Rectified Linear Unit
    /// </summary>
    public class ReLu : IActivation
    {
        public double Slope { get; }

        public ReLu(double slope = 0.0)
        {
            Slope = slope;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Slope * X : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Slope : 1.0);
        }

        public IActivation Clone()
        {
            return new ReLu(Slope);
        }
    }

    [Serializable]
    /// <summary>
    /// Randomized Rectified Linear Unit
    /// </summary>
    public class RReLu : IActivation
    {
        public double Slope { get; }

        public RReLu()
        {
            Slope = GRandom.NextDouble();
        }

        private RReLu(double slope)
        {
            Slope = slope;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Slope * X : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Slope : 1);
        }

        public IActivation Clone()
        {
            return new RReLu(Slope);
        }
    }

    [Serializable]
    /// <summary>
    /// Exponential Linear Unit
    /// </summary>
    public class ELU : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? (Exp(X) - 1.0) : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Exp(X) : 1.0);
        }

        public IActivation Clone()
        {
            return new ELU();
        }
    }

    [Serializable]
    /// <summary>
    /// Scaled Exponential Linear Unit
    /// </summary>
    public class SELU : IActivation
    {
        public double A { get; } = 1.0507009873554805;
        public double B { get; } = 1.6732632423543772;

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? A * B * (Exp(X) - 1.0) : A * X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? A * B * Exp(X) : A);
        }

        public IActivation Clone()
        {
            return new SELU();
        }
    }

    [Serializable]
    public class SoftPlus : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Log(1.0 + Exp(X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }

        public IActivation Clone()
        {
            return new SoftPlus();
        }
    }

    [Serializable]
    public class BentIdentity : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => (Sqrt(X * X + 1.0) - 1.0) / 2.0 + X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X / (2.0 * Sqrt(X * X + 1.0)) + 1.0);
        }

        public IActivation Clone()
        {
            return new BentIdentity();
        }
    }

    [Serializable]
    public class Swish : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X / (Exp(-X) + 1.0));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X =>
            {
                double exp = Exp(X);
                return exp * (1.0 + exp + X) / Pow(1.0 + exp, 2.0);
            });
        }

        public IActivation Clone()
        {
            return new Swish();
        }
    }

    [Serializable]
    public class SoftExponential : IActivation
    {
        public double Alpha { get; }

        private readonly Func<double, double> activation;
        private readonly Func<double, double> derivative;

        public SoftExponential(double alpha)
        {
            Alpha = alpha;

            if (alpha < 0.0)
            {
                activation = (X) => -Log(1.0 - alpha * (alpha + X)) / alpha;
                derivative = (X) => 1.0 / (1.0 - alpha * (alpha + X));
            }
            else if (alpha > 0.0)
            {
                activation = (X) => (Exp(alpha * X) - 1.0) / alpha + alpha;
                derivative = (X) => Exp(alpha * X);
            }
            else
            {
                activation = (X) => X;
                derivative = (X) => 1.0;
            }
        }

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => activation(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => derivative(X));
        }

        public IActivation Clone()
        {
            return new SoftExponential(Alpha);
        }
    }

    [Serializable]
    public class SoftClipping : IActivation
    {
        public double Alpha { get; }

        public SoftClipping(double alpha)
        {
            Alpha = alpha;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => 1.0 / Alpha * Log((1.0 + Exp(Alpha * X)) / (1.0 + Exp(Alpha * X - Alpha))));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 0.5 * Sinh(0.5 * Alpha) * (1.0 / Cosh(0.5 * Alpha * X)) * (1.0 / Cosh(0.5 * Alpha * (1.0 - X))));
        }

        public IActivation Clone()
        {
            return new SoftClipping(Alpha);
        }
    }

    [Serializable]
    public class Sinusoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Sin(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => Cos(X));
        }

        public IActivation Clone()
        {
            return new Sinusoid();
        }
    }

    [Serializable]
    public class Sinc : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X != 0.0 ? Sin(X) / X : 1.0);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X != 0.0 ? Cos(X) / X - Sin(X) / (X * X) : 0.0);
        }

        public IActivation Clone()
        {
            return new Sinc();
        }
    }

    [Serializable]
    public class Gaussian : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Exp(-X * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => -2.0 * X * Exp(-X * X));
        }

        public IActivation Clone()
        {
            return new Gaussian();
        }
    }

    [Serializable]
    public class Softmax : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] exps = vals.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E / sum);
        }

        public double[] Derivative(double[] vals)
        {
            double[] exps = vals.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E * (sum - E) / (sum * sum));
        }

        public IActivation Clone()
        {
            return new Softmax();
        }
    }
}

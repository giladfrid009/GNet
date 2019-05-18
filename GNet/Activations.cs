using System;
using static System.Math;
using GNet.GlobalRandom;
using GNet.Extensions;

namespace GNet
{
    // https://en.wikipedia.org/wiki/Activation_function

    public interface IActivation : ICloneable
    {
        double[] Activate(double[] vals);

        double[] Derivative(double[] vals);
    }
}

namespace GNet.Activations
{
    public class Identity : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 1.0);
        }

    public object Clone() => new Identity();
    }
        
    public class BinaryStep : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X > 0 ? 1.0 : 0.0);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 0.0);
        }

        public object Clone() => new BinaryStep();
    }

    public class Sigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => 1 / (1 + Exp(-X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => Exp(X) / Pow(1 + Exp(-X), 2));
        }

        public object Clone() => new Sigmoid();
    }

    public class HardSigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X < -2.5 ? 0 : X > 2.5 ? 1 : 0.2 * X + 0.5);            
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X < -2.5 || X > 2.5 ? 0 : 0.2);
        }

        public object Clone() => new HardSigmoid();
    }

    public class Tanh : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => Math.Tanh(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 1 / Pow(Cosh(X), 2));
        }

        public object Clone() => new Tanh();
    }

    public class LeCunTanh : IActivation
    {
        private readonly double a = 1.7159;
        private readonly double b = 2.0 / 3;

        public double[] Activate(double[] vals)
        {
            return vals.Map(X => a * Math.Tanh(b * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => a * b / Pow(Cosh(b * X), 2));            
        }

        public object Clone() => new LeCunTanh();
    }

    public class ArcTan : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => Atan(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 1 / (1 + X * X));           
        }

        public object Clone() => new ArcTan();
    }

    public class ArSinh : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => Asinh(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 1 / Sqrt(1 + X * X));
        }

        public object Clone() => new ArSinh();
    }

    public class SoftSign : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X / (1 + Abs(X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 1 / Pow(1 + Abs(X), 2));
        }

        public object Clone() => new SoftSign();
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
            return vals.Map(X => X / Sqrt(1 + alpha * X * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => Pow(X / Sqrt(1 + alpha * X * X), 3));
        }

        public object Clone() => new ISRU(alpha);
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
            return vals.Map(X => X >= 0 ? X : X / Sqrt(1 + alpha * X * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X >= 0 ? 1 : Pow(X / Sqrt(1 + alpha * X * X), 3));
        }

        public object Clone() => new ISRLU(alpha);
    }

    /// <summary>
    /// Square Nonlinearity 
    /// </summary>
    public class SQNL : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X =>
            {
                if (X > 2)
                    return 1;

                if (X >= 0 && X <= 2)
                    return X - X * X / 4;

                if (X >= -2 && X < 0)
                    return X - X * X / 4;

                return -1;
            });
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X > 0 ? 1 - X / 2 : 1 + X / 2);
        }

        public object Clone() => new SQNL();
    }    

    /// <summary>
    /// Rectified Linear Unit
    /// </summary>
    public class ReLu : IActivation
    {
        private readonly double slope;

        public ReLu(double slope = 0)
        {
            this.slope = slope;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X < 0 ? slope * X : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X < 0 ? slope : 1);
        }

        public object Clone() => new ReLu(slope);
    }

    /// <summary>
    /// Randomized Rectified Linear Unit
    /// </summary>
    public class RReLu : IActivation
    {
        private readonly double slope;

        public RReLu()
        {
            slope = GRandom.NextDouble();
        }

        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X < 0 ? slope * X : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X < 0 ? slope : 1);
        }

        public object Clone() => new RReLu();
    }

    /// <summary>
    /// Exponential Linear Unit
    /// </summary>
    public class ELU : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X < 0 ? (Exp(X) - 1) : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X < 0 ? Exp(X) : 1);
        }

        public object Clone() => new ELU();
    }

    /// <summary>
    /// Scaled Exponential Linear Unit
    /// </summary>
    public class SELU : IActivation
    {
        private readonly double a = 1.6732632423543772;
        private readonly double b = 1.0507009873554805;

        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X < 0 ? b * a * (Exp(X) - 1) : b * X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X < 0 ? b * a * Exp(X) : b);
        }

        public object Clone() => new SELU();
    }

    public class SoftPlus : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => Log(1 + Exp(X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 1 / (1 + Exp(-X)));
        }

        public object Clone() => new SoftPlus();
    }

    public class BentIdentity : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => (Sqrt(X * X + 1) - 1) / 2 + X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X / (2 * Sqrt(X * X + 1)) + 1);
        }

        public object Clone() => new BentIdentity();
    }

    public class Swish : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X / (Exp(-X) + 1));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X =>
            {
                var exp = Exp(X);
                return exp * (1 + exp + X) / Pow(1 + exp, 2);
            });
        }

        public object Clone() => new Swish();
    }

    public class SoftExponential : IActivation
    {
        private readonly double a;
        private readonly Func<double, double> act;
        private readonly Func<double, double> der;

        public SoftExponential(double a)
        {
            this.a = a;

            if (a < 0)
            {
                act = (X) => -Log(1 - a * (a + X)) / a;
                der = (X) => 1 / (1 - a * (a + X));
            }
            else if (a > 0)
            {
                act = (X) => (Exp(a * X) - 1) / a + a;
                der = (X) => Exp(a * X);           
            }
            else
            {
                act = (X) => X;
                der = (X) => 1;
            }
        }

        public double[] Activate(double[] vals)
        {
            return vals.Map(X => act(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => der(X));
        }

        public object Clone() => new SoftExponential(a);
    }

    public class SoftClipping : IActivation
    {
        private readonly double a;

        public SoftClipping(double a)
        {
            this.a = a;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Map(X => 1 / a * Log((1 + Exp(a * X)) / (1 + Exp(a * X - a))));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => 0.5 * Sinh(0.5 * a) * (1 / Cosh(0.5 * a * X)) * (1 / Cosh(0.5 * a * (1 - X))));
        }

        public object Clone() => new SoftClipping(a);
    }

    public class Sinusoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => Sin(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => Cos(X));
        }

        public object Clone() => new Sinusoid();
    }

    public class Sinc : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => X != 0 ? Sin(X) / X : 1);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => X != 0 ? Cos(X) / X - Sin(X) / (X * X) : 0);
        }

        public object Clone() => new Sinc();
    }

    public class Gaussian : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Map(X => Exp(-X * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Map(X => -2 * X * Exp(-X * X));
        }

        public object Clone() => new Gaussian();
    }

    public class Softmax : IActivation
    {
        public double[] Activate(double[] vals)
        {
            double[] exps = vals.Map(X => Exp(X));

            double sum = exps.Sum();

            return exps.Map(E => E / sum);
        }

        public double[] Derivative(double[] vals)
        {
            double[] exps = vals.Map(X => Exp(X));

            double sum = exps.Sum();

            return exps.Map(E => E * (sum - E) / (sum * sum));
        }

        public object Clone() => new Softmax();
    }   
}

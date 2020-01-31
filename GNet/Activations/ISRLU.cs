using System;
using static System.Math;

namespace GNet.Activations
{
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

        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X >= 0.0 ? X : X / Sqrt(1.0 + Alpha * X * X));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X >= 0.0 ? 1.0 : Pow(X / Sqrt(1.0 + Alpha * X * X), 3.0));
        }

        public IActivation Clone()
        {
            return new ISRLU(Alpha);
        }
    }
}

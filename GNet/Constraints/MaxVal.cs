using System;
using static System.Math;

namespace GNet.Constraints
{
    public class MaxVal : IConstraint
    {
        public double Max { get; }
        public double Mul { get; private set; }

        public MaxVal(double max = 2.0)
        {
            Max = max;
        }

        public void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector)
        {
            double norm = Sqrt(array.Sum(X => selector(X) * selector(X)));

            Mul = Clamp(norm, 0.0, Max) / norm;
        }

        public double Constrain(double X)
        {
            return Mul * X;
        }
    }
}

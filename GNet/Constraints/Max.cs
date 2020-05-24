using System;
using static System.Math;

namespace GNet.Constraints
{
    public class Max : IConstraint
    {
        public double MaxVal { get; }
        public double Mul { get; private set; }

        public Max(double maxVal = 2.0)
        {
            MaxVal = maxVal;
        }

        public void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector)
        {
            double norm = Sqrt(array.Sum(X => selector(X) * selector(X)));

            Mul = Clamp(norm, 0.0, MaxVal) / norm;
        }

        public double Apply(double X)
        {
            return Mul * X;
        }
    }
}

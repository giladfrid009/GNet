using System;
using static System.Math;

namespace GNet.Constraints
{
    public class MinMax : IConstraint
    {
        public double MinVal { get; }
        public double MaxVal { get; }
        public double Rate { get; }
        public double Mul { get; private set; }

        public MinMax(double minVal = 0.0, double maxVal = 1.0, double rate = 1.0)
        {
            MinVal = minVal;
            MaxVal = maxVal;
            Rate = rate;
        }

        public void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector)
        {
            double norm = Sqrt(array.Sum(X => selector(X) * selector(X)));

            Mul = Rate * Clamp(norm, MinVal, MaxVal) / norm + (1.0 - Rate);
        }

        public double Apply(double X)
        {
            return Mul * X;
        }
    }
}

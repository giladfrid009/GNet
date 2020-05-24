using System;
using static System.Math;

namespace GNet.Constraints
{
    public class MinMax : IConstraint
    {
        public double Min { get; }
        public double Max { get; }
        public double Rate { get; }
        public double Mul { get; private set; }

        public MinMax(double min = 0.0, double max = 1.0, double rate = 1.0)
        {
            Min = min;
            Max = max;
            Rate = rate;
        }

        public void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector)
        {
            double norm = Sqrt(array.Sum(X => selector(X) * selector(X)));

            Mul = Rate * Clamp(norm, Min, Max) / norm + (1.0 - Rate);
        }

        public double Constrain(double X)
        {
            return Mul * X;
        }
    }
}

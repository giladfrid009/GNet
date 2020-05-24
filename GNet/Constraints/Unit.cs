using System;
using static System.Math;

namespace GNet.Constraints
{
    public class Unit : IConstraint
    {
        public double Norm { get; private set; }

        public void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector)
        {
            Norm = Sqrt(array.Sum(X => selector(X) * selector(X)));
        }

        public double Constrain(double X)
        {
            return X / Norm;
        }
    }
}

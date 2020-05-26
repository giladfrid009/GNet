using System;

namespace GNet.Constraints
{
    public class None : IConstraint
    {
        public void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector)
        {
        }

        public double Apply(double X)
        {
            return X;
        }
    }
}

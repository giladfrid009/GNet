using System;

namespace GNet
{
    public interface IConstraint
    {
        void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector);

        double Constrain(double X);
    }
}

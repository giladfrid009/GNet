using System;

namespace GNet
{
    //todo: apply after param update
    public interface IConstraint
    {
        void UpdateParams<T>(ImmutableArray<T> array, Func<T, double> selector);

        double Apply(double X);
    }
}

using System;

namespace GNet
{
    public interface IExtender : ICloneable<IExtender>
    {
        ArrayImmutable<TOut> Select<TSource, TOut>(IArray<TSource> source, Func<TSource, int, TOut> selector);

        ArrayImmutable<TSource> Combine<TSource>(IArray<TSource> source, IArray<TSource> array, Func<TSource, TSource, TSource> selector);

        double Accumulate<TSource>(IArray<TSource> source, double seed, Func<double, TSource, double> accumulator);

        double Sum<TSource>(IArray<TSource> source, Func<TSource, double> selector);

        double Min(IArray<double> source);

        double Max(IArray<double> source);

        void ForEach<TSource>(IArray<TSource> source, Action<TSource, int> action);
    }
}

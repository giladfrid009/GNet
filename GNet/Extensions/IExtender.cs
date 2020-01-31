using System;

namespace GNet
{
    public interface IExtender : ICloneable<IExtender>
    {
        ArrayImmutable<TOut> Select<TSource, TOut>(IArray<TSource> source, Func<TSource, int, TOut> selector);

        ArrayImmutable<TSource> Combine<TSource>(IArray<TSource> source, IArray<TSource> array, Func<TSource, TSource, TSource> selector);

        TOut Accumulate<TSource, TOut>(IArray<TSource> source, TOut seed, Func<TOut, TSource, TOut> accumulator);

        double Sum<TSource>(IArray<TSource> source, Func<TSource, double> selector);

        double Avarage(IArray<double> source);

        double Min(IArray<double> source);

        double Max(IArray<double> source);

        void ForEach<TSource>(IArray<TSource> source, Action<TSource, int> action);
    }
}

using System;
using System.Collections.Generic;
using GNet.GlobalRandom;

namespace GNet.Extensions
{
    public static class Extensions
    {
        public static TSource[] DeepClone<TSource>(this TSource[] source)
        {
            return (TSource[])RecursiveClone(source);
        }

        private static object RecursiveClone(Array source)
        {
            Array newArr = (Array)Activator.CreateInstance(source.GetType(), source.Length);

            for (int i = 0; i < source.Length; i++)
            {
                var element = source.GetValue(i);

                if (element is Array)
                {
                    newArr.SetValue(RecursiveClone(element as Array), i);
                }
                else if (element is ICloneable)
                {
                    newArr.SetValue((element as ICloneable).Clone(), i);
                }
                else
                {
                    newArr.SetValue(element, i);
                }
            }

            return newArr;
        }

        public static TSource[] Flatten<TSource>(this Array source)
        {
            List<TSource> flattened = new List<TSource>();

            foreach (var element in source)
            {
                if (element is Array)
                {
                    flattened.AddRange(Flatten<TSource>(element as Array));
                }
                else if (element is TSource)
                {
                    flattened.Add((TSource)element);
                }
                else
                {
                    throw new ArrayTypeMismatchException("array element type mismatch");
                }
            }

            return flattened.ToArray();
        }

        public static void ClearRecursive(this Array source)
        {
            if (source.GetType().GetElementType().IsArray == false)
            {
                Array.Clear(source, 0, source.Length);
                return;
            }

            foreach (var element in source)
            {
                ClearRecursive((Array)element);
            }
        }

        public static TSource[] Shuffle<TSource>(this TSource[] source)
        {
            TSource[] shuffled = source.DeepClone();

            for (int i = 0; i < shuffled.Length; i++)
            {
                int index = GRandom.Next(i, shuffled.Length);

                var temp = shuffled[i];
                shuffled[i] = shuffled[index];
                shuffled[index] = temp;
            }

            return shuffled;
        }

        public static double Sum(this double[] source)
        {
            double sum = default;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum;
        }

        public static TOut[] Map<TSource, TOut>(this TSource[] source, Func<TSource, TOut> selector)
        {
            TOut[] mapped = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                mapped[i] = selector(source[i]);
            }

            return mapped;
        }

        public static TOut[] Combine<TSource, TOut>(this TSource[] source, TSource[] array, Func<TSource, TSource, TOut> selector)
        {
            int minIndex = Math.Min(source.Length, array.Length);

            TOut[] combined = new TOut[minIndex];

            for (int i = 0; i < minIndex; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return combined;
        }

        public static TSource Accumulate<TSource>(this TSource[] source, TSource seed, Func<TSource, TSource, TSource> accumulator)
        {
            TSource res = seed;

            for (int i = 0; i < source.Length; i++)
            {
                res = accumulator(res, source[i]);
            }

            return res;
        }

        public static void ForEach<TSource>(this TSource[] source, Action<TSource> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i]);
            }
        }
    }
}


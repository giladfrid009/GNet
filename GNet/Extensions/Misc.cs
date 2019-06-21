using System;

namespace GNet.Extensions.Misc
{
    public static class ExtensionsMisc
    {
        public static void Print<TSource>(this TSource val) where TSource : struct
        {
            Console.WriteLine(val);
        }

        public static void Print<TSource>(this TSource[] vals) where TSource : struct
        {
            for (int i = 0; i < vals.Length; i++)
            {
                Console.WriteLine(vals[i]);
            }
        }
    }
}

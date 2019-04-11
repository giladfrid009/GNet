using System;
using System.Collections.Generic;
using PNet.GlobalVars;

namespace PNet.Extensions
{
    public static class Extensions
    {
        public static T[] CopyByVal<T>(this T[] array)
        {
            T[] newArray = new T[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }

            return newArray;
        }

        public static T[][] CopyByVal<T>(this T[][] array)
        {
            T[][] newArray = new T[array.Length][];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i].CopyByVal();
            }

            return newArray;
        }

        public static T[][][] CopyByVal<T>(this T[][][] array)
        {
            T[][][] newArray = new T[array.Length][][];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i].CopyByVal();
            }

            return newArray;
        }

        public static T[] CopyByVal<T>(this T[] array, T forcedVal)
        {
            T[] newArray = new T[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = forcedVal;
            }

            return newArray;
        }

        public static T[][] CopyByVal<T>(this T[][] array, T forcedVal)
        {
            T[][] newArray = new T[array.Length][];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i].CopyByVal(forcedVal);
            }

            return newArray;
        }

        public static T[][][] CopyByVal<T>(this T[][][] array, T forcedVal)
        {
            T[][][] newArray = new T[array.Length][][];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i].CopyByVal(forcedVal);
            }

            return newArray;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rnd = Globals.Rnd;
            int upperBound = list.Count;

            while (upperBound > 1)
            {
                upperBound--;

                int index = rnd.Next(upperBound + 1);

                T tempVal = list[index];
                list[index] = list[upperBound];
                list[upperBound] = tempVal;
            }
        }

        public static double NextDouble(this Random rnd, double minValue, double maxValue)
        {
            return rnd.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}


using GNet.Extensions.Generic;
using GNet.Extensions.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double min = vals.Min();
            double max = vals.Max();
            double diff = max - min;

            if (diff == 0.0)
            {
                return vals.Select(X => 0.5);
            }

            return vals.Select(X => (X - min) / diff);
        }

        public INormalizer Clone()
        {
            return new MinMax();
        }
    }
}

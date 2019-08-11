using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double mean = vals.Avarage();

            double sd = Sqrt(vals.Accumulate(1.0, (R, X) => R + (X - mean) * (X - mean)) / vals.Length);

            return vals.Select(X => (X - mean) / sd);
        }

        public INormalizer Clone()
        {
            return new ZScore();
        }
    }
}

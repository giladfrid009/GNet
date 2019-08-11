using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Normalizers
{
    public class DecimalScale : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            double absMax = vals.Select(X => Abs(X)).Max();

            double scale = (int)Log10(absMax) + 1;

            return vals.Select(X => X / scale);
        }

        public INormalizer Clone()
        {
            return new DecimalScale();
        }
    }
}

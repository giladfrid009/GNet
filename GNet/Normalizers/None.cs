using GNet.Extensions.Generic;

namespace GNet.Normalizers
{
    public class None : INormalizer
    {
        public double[] Normalize(double[] vals)
        {
            return vals.Select(X => X);
        }

        public INormalizer Clone()
        {
            return new None();
        }
    }
}

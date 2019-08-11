using GNet.Extensions.Generic;
using GNet.Extensions.Math;

namespace GNet.OutTransformers
{
    public class BinaryMax : IOutTransformer
    {
        public double[] Transform(double[] output)
        {
            double max = output.Max();

            return output.Select(X => X != max ? 0.0 : 1);
        }

        public IOutTransformer Clone()
        {
            return new BinaryMax();
        }
    }
}

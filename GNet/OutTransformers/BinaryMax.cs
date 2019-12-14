using GNet.Extensions.Array.Generic;
using GNet.Extensions.Array.Math;
using GNet.Extensions.ShapedArray.Generic;
using GNet.Extensions.ShapedArray.Math;

namespace GNet.OutTransformers
{
    public class BinaryMax : IOutTransformer
    {
        public IOutTransformer Clone()
        {
            return new BinaryMax();
        }

        public ShapedArray<double> Transform(ShapedArray<double> output)
        {
            double max = output.Max();

            return output.Select(X => X != max ? 0.0 : 1);
        }
    }
}

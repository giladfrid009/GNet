namespace GNet.OutTransformers
{
    public class BinaryMax : IOutTransformer
    {
        public IOutTransformer Clone()
        {
            return new BinaryMax();
        }

        public ShapedArrayImmutable<double> Transform(ShapedArrayImmutable<double> output)
        {
            double max = output.Max();

            return output.Select(X => X != max ? 0.0 : 1);
        }
    }
}

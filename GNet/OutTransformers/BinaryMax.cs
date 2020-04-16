namespace GNet.OutTransformers
{
    public class BinaryMax : IOutTransformer
    {
        public ImmutableShapedArray<double> Transform(ImmutableShapedArray<double> output)
        {
            double max = output.Max();

            return output.Select(X => X != max ? 0.0 : 1.0);
        }
    }
}
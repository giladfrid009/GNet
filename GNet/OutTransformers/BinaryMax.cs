namespace GNet.OutTransformers
{
    public class BinaryMax : IOutTransformer
    {
        public ImmutableArray<double> Transform(ImmutableArray<double> output)
        {
            double max = output.Max();

            return output.Select(X => X != max ? 0.0 : 1.0);
        }
    }
}
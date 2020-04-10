namespace GNet.OutTransformers
{
    public class BinaryRound : IOutTransformer
    {
        public double Bound { get; }

        public BinaryRound(double bound = 0.5)
        {
            Bound = bound;
        }

        public ShapedArrayImmutable<double> Transform(ShapedArrayImmutable<double> output)
        {
            return output.Select(X => X < Bound ? 0.0 : 1.0);
        }
    }
}
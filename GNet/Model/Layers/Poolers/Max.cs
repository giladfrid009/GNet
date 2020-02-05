namespace GNet.Layers.Poolers
{
    public class Max : IPooler
    {
        public double Pool(ShapedArrayImmutable<double> vals)
        {
            return vals.Max();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            double max = vals.Max();

            return vals.Select(X => X == max ? 1.0 : 0.0);
        }

        public IPooler Clone()
        {
            return new Max();
        }
    }
}

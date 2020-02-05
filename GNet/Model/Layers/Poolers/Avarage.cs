namespace GNet.Layers.Poolers
{
    public class Avarage : IPooler
    {
        public double Pool(ShapedArrayImmutable<double> vals)
        {
            return vals.Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            int n = vals.Length;

            return vals.Select(X => X / n);
        }

        public IPooler Clone()
        {
            return new Avarage();
        }
    }
}

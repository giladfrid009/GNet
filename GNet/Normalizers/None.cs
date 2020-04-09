namespace GNet.Normalizers
{
    public class None : INormalizer
    {
        public None()
        {
        }

        public void UpdateParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector)
        {
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals;
        }
    }
}
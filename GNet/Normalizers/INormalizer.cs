namespace GNet
{
    public interface INormalizer
    {
        void UpdateParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector);

        ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals);
    }
}
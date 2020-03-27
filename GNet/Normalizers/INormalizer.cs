namespace GNet
{
    public interface INormalizer
    {
        void ExtractParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector);

        ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals);
    }
}
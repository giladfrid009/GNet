namespace GNet
{
    public static class Extensions
    {
        public static VArray<T> ToVArray<T>(this Array<T> array) where T : unmanaged
        {
            return new VArray<T>(array);
        }

        public static Tensor<T> ToTensor<T>(this Array<T> array, Shape shape) where T : unmanaged
        {
            return new Tensor<T>(shape, array);
        }
    }
}

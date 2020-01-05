namespace GNet
{
    public interface IShapedArray<T> : IArray<T>
    {
        Shape Shape { get; }
        T this[params int[] indices] { get; }
    }
}

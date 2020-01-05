namespace GNet
{
    public interface IShapedArray<T> : IArray<T>
    {
        Shape Shape { get; }
        new T this[int index] { get; }
        T this[params int[] indices] { get; }
    }
}

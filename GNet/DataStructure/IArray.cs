namespace GNet
{
    public interface IArray<T>
    {
        int Length { get; }
        T this[int i] { get; }
    }
}
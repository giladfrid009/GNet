namespace GNet
{
    public interface IArray<out T>
    {
        int Length { get; }
        T this[int i] { get; }
    }
}
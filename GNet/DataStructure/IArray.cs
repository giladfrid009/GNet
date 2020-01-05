namespace GNet
{
    public interface IArray<T>
    {
        int Length { get; }
        T this[int index] { get; }
    }


}

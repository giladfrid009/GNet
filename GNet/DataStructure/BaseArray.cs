namespace GNet
{
    public abstract class BaseArray<T>
    {
        protected BaseArray(int length)
        {
            Length = length;
        }

        public int Length { get; }
        public T this[int i] { get => InternalArray[i]; }

        protected abstract T[] InternalArray { get; }
    }
}
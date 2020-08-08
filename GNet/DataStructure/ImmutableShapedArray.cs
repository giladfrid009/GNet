using System;

namespace GNet
{
    [Serializable]
    public readonly struct ImmutableShapedArray<T>
    {
        public Shape Shape { get; }
        public int Length { get; }

        public T this[int i] => internalArray[i];
        public T this[params int[] idxs] => internalArray[Shape.FlattenIndices(idxs)];

        private readonly ImmutableArray<T> internalArray;

        private ImmutableShapedArray(in Shape shape, T[] array, bool asRef = false)
        {
            if (shape.Volume != array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shape), nameof(shape.Volume));
            }

            Shape = shape;
            Length = shape.Volume;

            internalArray = asRef ? ImmutableArray<T>.FromRef(array) : new ImmutableArray<T>(array);            
        }

        public ImmutableShapedArray(params T[] elements) : this(new Shape(elements.Length), elements, false)
        {
        }

        public ImmutableShapedArray(in Shape shape, params T[] elements) : this(shape, elements, false)
        {
        }

        public ImmutableShapedArray(in Shape shape, Func<T> element)
        {
            Shape = shape;
            Length = shape.Volume;

            internalArray = new ImmutableArray<T>(shape.Volume, element);
        }

        public static implicit operator ImmutableArray<T>(ImmutableShapedArray<T> shapedArr)
        {
            return shapedArr.internalArray;
        }

        public static bool operator !=(in ImmutableShapedArray<T> left, in ImmutableShapedArray<T> right)
        {
            return !(left == right);
        }

        public static bool operator ==(in ImmutableShapedArray<T> left, in ImmutableShapedArray<T> right)
        {
            return left.Equals(right);
        }

        public bool Equals(ImmutableShapedArray<T> other)
        {
            return (Shape, internalArray) == (other.Shape, other.internalArray);
        }

        public override bool Equals(object? obj)
        {
            return (obj is ImmutableArray<T> immArr) && Equals(immArr);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(internalArray, Shape);
        }

        public static ImmutableShapedArray<T> FromRef(in Shape shape, params T[] array)
        {
            return new ImmutableShapedArray<T>(shape, array, true);
        }      
    }
}
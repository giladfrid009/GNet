namespace GNet
{
    public interface INumOps<T> where T : unmanaged
    {
        T MinValue { get; }
        T MaxValue { get; }

        T Add(T left, T right);
        T Sub(T left, T right);
        T Div(T left, T right);
        T Mul(T left, T right);

        T Min(T left, T right);
        T Max(T left, T right);

        bool Equals(T left, T right);

        TOther To<TOther>(T value) where TOther : unmanaged;
        T From<TOther>(TOther value) where TOther : unmanaged;
    }
}

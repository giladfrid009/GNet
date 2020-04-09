namespace GNet
{
    public interface IConvLayer : ILayer
    {
        ArrayImmutable<int> Strides { get; }
        ArrayImmutable<int> Paddings { get; }
        Shape InputShape { get; }
        Shape PaddedShape { get; }
        Shape KernelShape { get; }
    }
}

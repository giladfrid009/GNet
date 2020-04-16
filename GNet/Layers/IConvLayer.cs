namespace GNet
{
    public interface IConvLayer : ILayer
    {
        ImmutableArray<int> Strides { get; }
        ImmutableArray<int> Paddings { get; }
        Shape InputShape { get; }
        Shape PaddedShape { get; }
        Shape KernelShape { get; }
    }
}

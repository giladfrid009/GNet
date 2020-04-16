
namespace GNet.Activations
{
    public class HardTanh : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < -1.0 ? -1 : X > 1.0 ? 1.0 : X);
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < -1.0 || X > 1.0 ? 0.0 : 1.0);
        }
    }
}

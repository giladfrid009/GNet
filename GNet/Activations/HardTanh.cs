
namespace GNet.Activations
{
    public class HardTanh : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => X < -1.0 ? -1 : X > 1.0 ? 1.0 : X);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => X < -1.0 || X > 1.0 ? 0.0 : 1.0);
        }
    }
}

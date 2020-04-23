namespace GNet.Losses.Regression
{
    public class MSE : ILoss
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) * (T - O)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 2.0 * (O - T));
        }
    }
}
namespace GNet.Optimizers
{
    public interface IDecay
    {
        double Compute(double X, int T);
    }
}
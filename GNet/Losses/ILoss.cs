namespace GNet
{
    public interface ILoss : ICloneable<ILoss>
    {
        double Compute(double[] targets, double[] outputs);

        double[] Derivative(double[] targets, double[] outputs);
    }
}

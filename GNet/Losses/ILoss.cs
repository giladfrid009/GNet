namespace GNet
{
    public interface ILoss
    {
        //TODO: GO OVER ALL LOSSES
        //TODO: VERIFY ALL DERIVATIVES ARE CORRECT
        //TODO: UNDERSTAND CROSSENTROPY LOSS.
        double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs);

        ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs);
    }
}
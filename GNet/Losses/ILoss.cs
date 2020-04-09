namespace GNet
{
    public interface ILoss
    {
        //TODO: GO OVER ALL LOSSES
        //TODO: VERIFY ALL DERIVATIVES ARE CORRECT
        //TODO: UNDERSTAND CROSSENTROPY LOSS.
        double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs);

        ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs);
    }
}
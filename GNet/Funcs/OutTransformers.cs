using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet
{
    public interface IOutTransformer : ICloneable<IOutTransformer>
    {
        double[] Transform(double[] output);
    }
}

namespace GNet.OutTransformers
{
    public class BinaryRound : IOutTransformer
    {
        public double Bound { get; protected set; }

        public BinaryRound(double bound = 0.5)
        {
            Bound = bound;
        }

        public double[] Transform(double[] output)
        {
            return output.Select(X => X < Bound ? 0.0 : 1);
        }

        public IOutTransformer Clone() => new BinaryRound(Bound);
    }

    public class BinaryMax : IOutTransformer
    {
        public double[] Transform(double[] output)
        {
            double max = output.Max();

            return output.Select(X => X != max ? 0.0 : 1);
        }

        public IOutTransformer Clone() => new BinaryMax();
    }
}

namespace GNet.OutTransformers.Losses
{
    public class BinaryRoundLoss : BinaryRound, ILoss
    {
        public BinaryRoundLoss(double bound = 0.5)
        {
            Bound = bound;
        }

        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            throw new NotSupportedException("This loss can't be used in backpropogation.");
        }

        public new ILoss Clone() => new BinaryRoundLoss(Bound);
    }

    public class BinaryMaxLoss : BinaryMax, ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            throw new NotSupportedException("This loss can't be used in backpropogation.");
        }

        public new ILoss Clone() => new BinaryMaxLoss();
    }
}

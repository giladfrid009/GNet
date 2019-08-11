using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet.OutTransformers.Losses
{
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

        public new ILoss Clone()
        {
            return new BinaryMaxLoss();
        }
    }
}

using System;
using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Softmax : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            ShapedArrayImmutable<double> exps = vals.Select(X => Exp(X));

            double sum = exps.Sum(x => x);

            return exps.Select(E => E / sum);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            ShapedArrayImmutable<double> exps = vals.Select(X => Exp(X));

            double sum = exps.Sum(x => x);

            return exps.Select(E => E * (sum - E) / (sum * sum));
        }

        public IActivation Clone()
        {
            return new Softmax();
        }
    }
}

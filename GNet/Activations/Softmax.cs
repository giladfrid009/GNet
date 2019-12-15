using GNet.Extensions.Array;
using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Softmax : IActivation
    {
        public ShapedArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            ShapedArray<double> exps = vals.Select(X => Exp(X));

            double sum = exps.Sum(x => x);

            return exps.Select(E => E / sum);
        }

        public ShapedArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            ShapedArray<double> exps = vals.Select(X => Exp(X));

            double sum = exps.Sum(x => x);

            return exps.Select(E => E * (sum - E) / (sum * sum));
        }

        public IActivation Clone()
        {
            return new Softmax();
        }
    }
}

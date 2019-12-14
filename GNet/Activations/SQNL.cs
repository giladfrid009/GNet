using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using System;

namespace GNet.Activations
{
    [Serializable]
    /// <summary>
    /// Square Nonlinearity 
    /// </summary>
    public class SQNL : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X =>
            {
                if (X > 2.0)
                {
                    return 1.0;
                }

                if (X >= 0.0 && X <= 2.0)
                {
                    return X - X * X / 4.0;
                }

                if (X >= -2.0 && X < 0.0)
                {
                    return X - X * X / 4.0;
                }

                return -1.0;
            });
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => X > 0.0 ? 1.0 - X / 2.0 : 1.0 + X / 2.0);
        }

        public IActivation Clone()
        {
            return new SQNL();
        }
    }
}

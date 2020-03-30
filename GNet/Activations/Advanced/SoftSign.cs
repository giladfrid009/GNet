﻿using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class SoftSign : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X / (1.0 + Abs(X)));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 1.0 / Pow(1.0 + Abs(X), 2));
        }
    }
}
﻿using GNet.Extensions.Generic;

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

        public IOutTransformer Clone()
        {
            return new BinaryRound(Bound);
        }
    }
}
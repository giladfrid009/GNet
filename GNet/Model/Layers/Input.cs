using System;
using GNet.Extensions.Generic;

namespace GNet.Layers
{
    [Serializable]
    public class Input : Base
    {
        public Input(int length) : base(length, new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero())
        {

        }

        public void SetInput(double[] values)
        {
            if (values.Length != Length)
                throw new ArgumentOutOfRangeException("Values length mismatch.");

            Neurons.ForEach((N, i) =>
            {
                N.Value = values[i];
                N.ActivatedValue = values[i];
            });
        }

        public override Base Clone() => new Input(Length);
    }
}

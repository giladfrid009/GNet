using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Hidden : Base
    {
        public Hidden(int length, IActivation activation, IInitializer weightInit, IInitializer biasInit) : base(length, activation, weightInit, biasInit)
        {
            
        }             

        public void FeedBackward(IOptimizer optimizer, int epoch)
        {
            double[] actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Accumulate(0.0, (R, S) => R + S.Weight * S.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });

            optimizer.Optimize(Neurons, epoch);
        }

        public virtual void Update()
        {
            Neurons.ForEach(N =>
            {
                N.Bias += N.BatchBias;
                N.BatchBias = 0.0;

                N.InSynapses.ForEach(S =>
                {
                    S.Weight += S.BatchWeight;
                    S.BatchWeight = 0.0;
                });
            });
        }

        public override Base Clone() => new Hidden(Length, Activation, WeightInit, BiasInit);
    }
}

using System;
using GNet.Extensions.Generic;

namespace GNet.Layers
{
    [Serializable]
    public class Output : Base
    {
        public Output(int length, IActivation activation, IInitializer weightInit, IInitializer biasInit) : base(length, activation, weightInit, biasInit)
        {

        }

        public void FeedBackward(IOptimizer optimizer, ILoss loss, double[] targets, int epoch)
        {
            if (loss is IOutTransformer)
                throw new ArgumentException("This loss doesn't support backpropogation.");

            double[] actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            double[] lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            double[] grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });

            optimizer.Optimize(Neurons, epoch);
        }

        public void Update()
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

        public override Base Clone() => new Output(Length, Activation, WeightInit, BiasInit);
    }
}

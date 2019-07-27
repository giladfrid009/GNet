using System;
using GNet.Extensions.Generic;
using GNet.Extensions.Math;

namespace GNet.Layers
{
    [Serializable]
    public abstract class Base : ICloneable<Base>
    {
        public Neuron[] Neurons { get; } = new Neuron[0];
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public int Length { get; }

        public Base(int length, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("Length must be positive.");

            Length = length;
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();

            Neurons = new Neuron[Length];

            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron();
            }
        }

        protected void Connect(Base inLayer)
        {
            inLayer.Neurons.ForEach(N => N.OutSynapses = new Synapse[Length]);
            Neurons.ForEach(N => N.InSynapses = new Synapse[inLayer.Length]);

            Neurons.ForEach((outN, i) =>
            {
                inLayer.Neurons.ForEach((inN, j) =>
                {
                    Synapse W = new Synapse(inN, outN);
                    inN.OutSynapses[i] = W;
                    outN.InSynapses[j] = W;
                });
            });
        }

        public virtual void Init(Base inLayer)
        {
            Connect(inLayer);

            Neurons.ForEach(N =>
            {
                N.Bias = BiasInit.Init(inLayer.Length, Length);
                N.InSynapses.ForEach(W => W.Weight = WeightInit.Init(inLayer.Length, Length));
            });
        }

        public void FeedForward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Accumulate(0.0, (R, S) => R + S.Weight * S.InNeuron.ActivatedValue));

            double[] activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }        

        public abstract Base Clone();
    }
}

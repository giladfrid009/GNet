using System;
using GNet.GlobalRandom;
using GNet.Extensions.Generic;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : Hidden
    {
        public double DropProb { get; }

        private Synapse[,] droppedCache;
        private readonly Synapse disconnectedSynapse;

        public Dropout(int length, IActivation activation, IInitializer weightInit, IInitializer biasInit, double dropProb) : base(length, activation, weightInit, biasInit)
        {
            if (dropProb < 0 || DropProb > 1)
                throw new ArgumentOutOfRangeException("DropProb must be in range 0 - 1.");

            DropProb = dropProb;
            disconnectedSynapse = new Synapse(new Neuron(), new Neuron());
        }

        public override void Init(Base inLayer)
        {
            base.Init(inLayer);

            droppedCache = new Synapse[Length, inLayer.Length];

            Drop();
        }

        public override void Update()
        {
            base.Update();
            Drop();
        }

        // todo: is it right?
        private void Drop()
        {
            Neurons.ForEach((N, i) =>
            {
                N.InSynapses.ForEach((S, j) =>
                {
                    if (GRandom.NextDouble() < DropProb)
                    {
                        if (droppedCache[i, j] == null)
                            droppedCache[i, j] = S;

                        N.InSynapses[j] = disconnectedSynapse;                
                    }
                    else if (droppedCache[i, j] != null)
                    {
                        N.InSynapses[j] = droppedCache[i, j];
                        droppedCache[i, j] = null;
                    }
                });
            });
        }

        public override Base Clone() => new Hidden(Length, Activation, WeightInit, BiasInit);
    }
}

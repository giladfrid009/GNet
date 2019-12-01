using GNet.Extensions.Generic;
using GNet.GlobalRandom;
using System;

namespace GNet
{
    [Serializable]
    public class Dropout : Dense
    {
        public double DropChance { get; }

        private Synapse?[,]? droppedCache;
        private readonly Synapse blankSynapse;

        public Dropout(int length, IActivation activation, IInitializer weightInit, IInitializer biasInit, double dropChance) : base(length, activation, weightInit, biasInit)
        {
            if (dropChance < 0.0 || dropChance > 1.0)
            {
                throw new ArgumentOutOfRangeException("DropProbability must be in range (0 - 1).");
            }

            DropChance = dropChance;
            blankSynapse = new Synapse(new Neuron(), new Neuron());
        }

        public override void Connect(Dense inLayer)
        {
            base.Connect(inLayer);

            droppedCache = new Synapse[Length, inLayer.Length];

            Drop();
        }

        public override void Update()
        {
            base.Update();

            Drop();
        }

        private void Drop()
        {
            if (droppedCache == null)
            {
                throw new InvalidOperationException("Layer hasn't been connected.");
            }

            Neurons.ForEach((N, i) =>
            {
                N.InSynapses.ForEach((S, j) =>
                {
                    if (GRandom.NextDouble() < DropChance)
                    {
                        if (droppedCache[i, j] == null)
                        {
                            droppedCache[i, j] = S;
                        }

                        N.InSynapses[j] = blankSynapse;
                    }
                    else if (droppedCache[i, j] != null)
                    {
                        N.InSynapses[j] = droppedCache[i, j] ?? throw new Exception();
                        droppedCache[i, j] = null;
                    }
                });
            });
        }

        public override Dense Clone()
        {
            return new Dropout(Length, Activation, WeightInit, BiasInit, DropChance);
        }
    }
}

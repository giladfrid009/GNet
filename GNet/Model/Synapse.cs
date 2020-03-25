﻿using System;

namespace GNet.Model
{
    [Serializable]
    public class Synapse
    {
        public Neuron InNeuron { get; }
        public Neuron OutNeuron { get; }
        public virtual double Weight { get; set; }

        public double BatchWeight;
        public double Gradient;
        public double Cache1;
        public double Cache2;

        public Synapse(Neuron inNeuron, Neuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }

        public void CopyParams(Synapse other)
        {
            Weight = other.Weight;
            Gradient = other.Gradient;
            Cache1 = other.Cache1;
            Cache2 = other.Cache2;
            BatchWeight = other.BatchWeight;
        }
    }
}
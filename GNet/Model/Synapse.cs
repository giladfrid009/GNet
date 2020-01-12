﻿using System;

namespace GNet
{
    [Serializable]
    public class Synapse
    {
        // todo: come with better naming.
        public OutNeuron InNeuron { get; }
        public InNeuron OutNeuron { get; }
        public double Weight { get; set; }

        public double Gradient;
        public double Cache1;
        public double Cache2;
        public double BatchWeight;

        public Synapse(OutNeuron inNeuron, InNeuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }

        public void CloneVals(Synapse other)
        {
            Weight = other.Weight;
            Gradient = other.Gradient;
            Cache1 = other.Cache1;
            Cache2 = other.Cache2;
            BatchWeight = other.BatchWeight;
        }
    }
}

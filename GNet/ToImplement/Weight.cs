using System;
using System.Collections.Generic;
using System.Text;

namespace GNet
{
    class Weight
    {
        public double Value;
        public Neuron inNeuron { get; }
        public Neuron outNeuron { get; }

        public Weight(Neuron inNeuron, Neuron outNeuron)
        {
            this.inNeuron = inNeuron;
            this.outNeuron = outNeuron;
        }
    }
}

using System;

namespace GNet.Model.Conv
{
    [Serializable]
    public class CSynapse : Synapse
    {
        public SharedVal<double> KernelWeight { get; set; }
        public override double Weight { get => KernelWeight.Value; set => KernelWeight.Value = value; }

        public CSynapse(Neuron inNeuron, Neuron outNeuron) : base(inNeuron, outNeuron)
        {
            KernelWeight = new SharedVal<double>();
        }
    }
}

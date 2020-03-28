using System;

namespace GNet.Model.Convolutional
{
    [Serializable]
    public class CNeuron : Neuron
    {
        public SharedVal<double> KernelBias { get; set; }
        public override double Bias { get => KernelBias.Value; set => KernelBias.Value = value; }

        public CNeuron()
        {
            KernelBias = new SharedVal<double>();
        }
    }
}
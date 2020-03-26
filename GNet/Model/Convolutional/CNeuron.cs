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

        public override Neuron Clone()
        {
            return new CNeuron()
            {
                Bias = Bias,
                Value = Value,
                Gradient = Gradient,
                Cache1 = Cache1,
                Cache2 = Cache2,
                BatchBias = BatchBias,
            };
        }
    }
}

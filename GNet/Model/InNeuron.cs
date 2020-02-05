using System;

namespace GNet.Model
{
    [Serializable]
    public class InNeuron : ICloneable<InNeuron>
    {
        public ShapedArrayImmutable<Synapse> InSynapses { get; set; }
        public double Bias { get; set; }

        public double Value;
        public double Gradient;
        public double Cache1;
        public double Cache2;
        public double BatchBias;

        public InNeuron()
        {

        }

        public InNeuron Clone()
        {
            return new InNeuron()
            {
                Bias = Bias,
                Value = Value,
                Gradient = Gradient,
                Cache1 = Cache1,
                Cache2 = Cache2,
                BatchBias = BatchBias
            };
        }
    }
}


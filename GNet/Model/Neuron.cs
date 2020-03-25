using System;

namespace GNet.Model
{
    [Serializable]
    public class Neuron : ICloneable<Neuron>
    {
        public ShapedArrayImmutable<Synapse> InSynapses { get; set; }
        public ShapedArrayImmutable<Synapse> OutSynapses { get; set; }
        public virtual double Bias { get; set; }

        public double Value;
        public double ActivatedValue;
        public double BatchBias;
        public double Gradient;
        public double Cache1;
        public double Cache2;

        public Neuron()
        {
        }

        public virtual Neuron Clone()
        {
            return new Neuron()
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
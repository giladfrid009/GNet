using System;

namespace GNet
{
    [Serializable]
    public class Neuron
    {
        public ShapedArrayMutable<Synapse> InSynapses { get; set; }
        public ShapedArrayMutable<Synapse> OutSynapses { get; set; }
        public double Bias { get; set; }

        public Neuron()
        {
            InSynapses = new ShapedArrayMutable<Synapse>(new Shape(0));
            OutSynapses = new ShapedArrayMutable<Synapse>(new Shape(0));
        }

        public double Value;
        public double ActivatedValue;
        public double Gradient;
        public double Cache1;
        public double Cache2;
        public double BatchBias;

        public void CloneVals(Neuron other)
        {
            Value = other.Value;
            ActivatedValue = other.ActivatedValue;
            Bias = other.Bias;
            Gradient = other.Gradient;
            Cache1 = other.Cache1;
            Cache2 = other.Cache2;
            BatchBias = other.BatchBias;
        }
    }
}

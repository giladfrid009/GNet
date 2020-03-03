using System;
using GNet.Model;

namespace GNet.Layers.Internal
{
    [Serializable]
    public class ConvIn : ILayer
    {
        public ShapedArrayImmutable<Neuron> Neurons { get; protected set; }
        public Shape Shape { get; }
        public bool IsTrainable { get; } = false;

        public ConvIn(Shape shape)
        {
            Shape = shape;           
            Neurons = new ShapedArrayImmutable<Neuron>(shape, () => new Neuron());
        }

        public virtual void Connect(ILayer inLayer)
        {
            Neurons.ForEach((N, i) =>
            {
                N.InSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), new Synapse(inLayer.Neurons[i], N));
                inLayer.Neurons[i].OutSynapses = N.InSynapses;
            });
        }

        public void Initialize()
        {
            Neurons.ForEach(N =>
            {
                N.Bias = 0;
                N.InSynapses.ForEach(S => S.Weight = 1);
            });
        }

        public virtual void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ArgumentOutOfRangeException("values shape mismatch.");
            }

            Neurons.ForEach((N, i) =>
            {
                N.Value = values[i];
                N.ActivatedValue = values[i];
            });
        }

        public virtual void Forward()
        {
            Neurons.ForEach(N =>
            {
                N.Value = N.InSynapses[0].InNeuron.ActivatedValue;
                N.ActivatedValue = N.InSynapses[0].InNeuron.ActivatedValue;
            });            
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            throw new NotSupportedException("This layer can't be used as output layer.");
        }

        public void CalcGrads()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public virtual void Update()
        {
            throw new NotSupportedException("This is a constant layer.");
        }

        public virtual ILayer Clone()
        {
            return new ConvIn(Shape)
            {
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}

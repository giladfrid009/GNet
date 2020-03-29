using System;
using GNet.Model;
using GNet.Utils;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : ILayer
    {
        public ShapedArrayImmutable<Neuron> Neurons { get; protected set; }
        public Shape Shape { get; }
        public bool IsTrainable { get; } = false;

        public double DropChance
        {
            get => dropChance;
            set
            {
                if (dropChance < 0 || dropChance > 1)
                {
                    throw new ArgumentOutOfRangeException("DropChance must be in range (0 - 1).");
                }

                dropChance = value;
            }
        }

        private ShapedArrayImmutable<bool> dropArray;
        private double dropChance;

        public Dropout(Shape shape, double dropChance)
        {
            Shape = shape;
            DropChance = dropChance;
            Neurons = new ShapedArrayImmutable<Neuron>(shape, () => new Neuron());
            dropArray = new ShapedArrayImmutable<bool>();
        }

        public virtual void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != Shape)
            {
                throw new ArgumentException("InLayer shape mismatch.");
            }

            Neurons.ForEach((N, i) =>
            {
                var S = new Synapse(inLayer.Neurons[i], N);

                N.InSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), S);
                inLayer.Neurons[i].OutSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), S);
            });
        }

        public void Initialize()
        {
            Update();

            Neurons.ForEach(N =>
            {
                N.Bias = 0;
                N.InSynapses[0].Weight = 1;
            });
        }

        public virtual void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ArgumentOutOfRangeException("Values shape mismatch.");
            }

            Neurons.ForEach((N, i) =>
            {
                N.Value = values[i];
                N.ActivatedValue = dropArray[i] ? 0 : N.Value;
            });
        }

        public virtual void Forward()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Value = N.InSynapses[0].InNeuron.ActivatedValue;
                N.ActivatedValue = dropArray[i] ? 0 : N.Value;
            });
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ArgumentException("Targets shape mismatch.");
            }

            if (loss is IOutTransformer)
            {
                throw new ArgumentException($"{nameof(loss)} loss doesn't support backpropogation.");
            }

            ShapedArrayImmutable<double> grads = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));

            Neurons.ForEach((N, i) => N.Gradient = dropArray[i] ? 0 : grads[i]);
        }

        public void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = dropArray[i] ? 0 : N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public virtual void Update()
        {
            dropArray = new ShapedArrayImmutable<bool>(Shape, () => GRandom.NextDouble(0, 1) <= DropChance);
        }
    }
}
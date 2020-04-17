using System;
using GNet.Model;
using GNet.Utils;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : ILayer
    {
        public ImmutableShapedArray<Neuron> Neurons { get; }
        public Shape Shape { get; }
        public bool IsTrainable { get; } = false;

        public double DropChance
        {
            get => dropChance;
            set
            {
                if (dropChance < 0 || dropChance > 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                dropChance = value;
            }
        }

        private ImmutableArray<bool> dropArray;
        private double dropChance;

        public Dropout(Shape shape, double dropChance)
        {
            Shape = shape;
            DropChance = dropChance;
            Neurons = new ImmutableShapedArray<Neuron>(shape, () => new Neuron());
            dropArray = new ImmutableArray<bool>();
        }

        public virtual void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            Neurons.ForEach((N, i) =>
            {
                var S = new Synapse(inLayer.Neurons[i], N);

                N.InSynapses = new ImmutableArray<Synapse>(S);
                inLayer.Neurons[i].OutSynapses = new ImmutableArray<Synapse>(S);
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

        public virtual void Input(ImmutableShapedArray<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
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

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            ImmutableArray<double> grads = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));

            Neurons.ForEach((N, i) => N.Gradient = dropArray[i] ? 0 : grads[i]);
        }

        public void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = dropArray[i] ? 0 : N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public void Optimize(IOptimizer optimizer)
        {
        }

        public void Update()
        {
            dropArray = new ImmutableArray<bool>(Shape.Volume, () => GRandom.NextDouble(0, 1) <= DropChance);
        }
    }
}
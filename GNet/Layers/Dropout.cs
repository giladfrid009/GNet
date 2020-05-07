using System;
using GNet.Model;
using GNet.Utils;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : ILayer
    {
        public ImmutableArray<Neuron> Neurons { get; }
        public Shape Shape { get; }
        public double DropChance { get; }

        private ImmutableArray<bool> dropArray;

        public Dropout(Shape shape, double dropChance = 0.1)
        {
            if (dropChance < 0 || dropChance > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dropChance));
            }

            Shape = shape;
            DropChance = dropChance;
            Neurons = new ImmutableArray<Neuron>(shape.Volume, () => new Neuron());
            dropArray = new ImmutableArray<bool>();
        }

        public void Connect(ILayer inLayer)
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
                N.Bias = 0.0;
                N.InSynapses[0].Weight = 1.0;
            });
        }

        public void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            Neurons.ForEach((N, i) => N.InVal = values[i]);

            if (isTraining)
            {
                Neurons.ForEach((N, i) => N.OutVal = dropArray[i] ? 0 : N.InVal);
            }
            else
            {
                Neurons.ForEach((N, i) => N.OutVal = N.InVal);
            }
        }

        public void Forward(bool isTraining)
        {
            Neurons.ForEach((N, i) => N.InVal = N.InSynapses[0].InNeuron.OutVal);

            if (isTraining)
            {
                Neurons.ForEach((N, i) => N.OutVal = dropArray[i] ? 0 : N.InVal);
            }
            else
            {
                Neurons.ForEach((N, i) => N.OutVal = N.InVal);
            }
        }   

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            Neurons.ForEach((N, i) => N.Gradient = dropArray[i] ? 0 : loss.Derivative(targets[i], N.OutVal));
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
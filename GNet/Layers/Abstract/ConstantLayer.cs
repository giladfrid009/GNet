using GNet.Model;
using System;

namespace GNet.Layers
{
    /// <summary>
    /// Base class for a non trainable layer without activation function.
    /// </summary>
    [Serializable]
    public abstract class ConstantLayer : Layer
    {
        public override ImmutableArray<Neuron> Neurons { get; }

        protected ConstantLayer(in Shape shape) : base(shape)
        {
            Neurons = new ImmutableArray<Neuron>(shape.Volume, () => new Neuron());
        }

        public sealed override void CalcGrads(ILoss loss, in ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            for (int i = 0; i < Neurons.Length; i++)
            {
                Neuron N = Neurons[i];
                N.Gradient = loss.Derivative(targets[i], N.OutVal);
            }
        }

        public sealed override void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public sealed override void Optimize(IOptimizer optimizer)
        {
        }

        public override void Update()
        {
        }
    }
}
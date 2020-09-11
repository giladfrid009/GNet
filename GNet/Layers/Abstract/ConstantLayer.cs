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
        public override Array<Neuron> Neurons { get; }

        protected ConstantLayer(Shape shape) : base(shape)
        {
            Neurons = new Array<Neuron>(shape.Volume, () => new Neuron());
        }

        public sealed override void CalcGrads(ILoss loss, Array<double> targets)
        {
            Neurons.ForEach((N, i) => N.Gradient = loss.Derivative(targets[i], N.OutVal));
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
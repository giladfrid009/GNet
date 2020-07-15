using GNet.Model;
using System;

namespace GNet.Layers
{
    [Serializable]
    public abstract class ConstantLayer : Layer
    {
        public override ImmutableArray<Neuron> Neurons { get; }

        protected ConstantLayer(Shape shape) : base(shape)
        {
            Neurons = new ImmutableArray<Neuron>(shape.Volume, () => new Neuron());
        }

        public override sealed void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            Neurons.ForEach((N, i) => N.Gradient = loss.Derivative(targets[i], N.OutVal));
        }

        public override sealed void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public override sealed void Optimize(IOptimizer optimizer)
        {
        }

        public override void Update()
        {
        }
    }
}

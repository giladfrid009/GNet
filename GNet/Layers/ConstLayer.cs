using GNet.Model;

namespace GNet.Layers
{
    public abstract class ConstLayer : ILayer
    {
        public ImmutableArray<Neuron> Neurons { get; }
        public Shape Shape { get; }

        protected ConstLayer(Shape shape)
        {
            Shape = shape;
            Neurons = new ImmutableArray<Neuron>(shape.Volume, () => new Neuron());
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            Neurons.ForEach((N, i) => N.Gradient = loss.Derivative(targets[i], N.OutVal));
        }

        public void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public virtual void Optimize(IOptimizer optimizer)
        {
        }

        public virtual void Update()
        {
        }

        public abstract void Connect(ILayer inLayer);

        public abstract void Initialize();

        public abstract void Forward(bool isTraining);
    }
}

using GNet.Model;

namespace GNet.Layers
{
    public class Reshape : ILayer
    {
        public ImmutableArray<Neuron> Neurons { get; }
        public Shape Shape { get; }

        public Reshape(Shape shape)
        {
            Shape = shape;
            Neurons = new ImmutableArray<Neuron>(shape.Volume, () => new Neuron());
        }

        public virtual void Connect(ILayer inLayer)
        {
            if (inLayer.Shape.Volume != Shape.Volume)
            {
                throw new ShapeMismatchException($"{nameof(inLayer)} shape volume mismatch.");
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
            Neurons.ForEach(N =>
            {
                N.Bias = 0.0;
                N.InSynapses[0].Weight = 1.0;
            });
        }

        public virtual void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            if (values.Shape.Volume != Shape.Volume)
            {
                throw new ShapeMismatchException($"{nameof(values)} shape volume mismatch.");
            }

            Neurons.ForEach((N, i) =>
            {
                N.InVal = values[i];
                N.OutVal = N.InVal;
            });
        }

        public virtual void Forward(bool isTraining)
        {
            Neurons.ForEach((N, i) =>
            {
                N.InVal = N.InSynapses[0].InNeuron.OutVal;
                N.OutVal = N.InVal;
            });
        }

        public virtual void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            Neurons.ForEach((N, i) => N.Gradient = loss.Derivative(targets[i], N.OutVal));
        }

        public virtual void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public void Optimize(IOptimizer optimizer)
        {
        }

        public virtual void Update()
        {
        }
    }
}

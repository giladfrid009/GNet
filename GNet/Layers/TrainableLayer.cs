using System;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public abstract class TrainableLayer<TNeuron> : ILayer where TNeuron : Neuron, new()
    {
        public ShapedArrayImmutable<Neuron> Neurons { get; }
        public Shape Shape { get; }
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public bool IsTrainable { get; set; } = true;

        protected TrainableLayer(Shape shape, IActivation activation, IInitializer biasInit, IInitializer weightInit)
        {
            Shape = shape;
            Activation = activation;
            BiasInit = biasInit;
            WeightInit = weightInit;

            Neurons = new ShapedArrayImmutable<Neuron>(shape, () => new TNeuron());
        }

        public void Forward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.ActivatedValue));

            ShapedArrayImmutable<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
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

            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            ShapedArrayImmutable<double> lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            ShapedArrayImmutable<double> grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void CalcGrads()
        {
            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void Optimize(IOptimizer optimizer)
        {
            if (IsTrainable == false)
            {
                return;
            }

            Neurons.ForEach(N =>
            {
                N.BatchDelta += optimizer.Optimize(N);
                N.InSynapses.ForEach(S => S.BatchDelta += optimizer.Optimize(S));
            });
        }

        public void Update()
        {
            if(IsTrainable == false)
            {
                return;
            }

            Neurons.ForEach(N =>
            {
                N.Bias += N.BatchDelta;
                N.BatchDelta = 0.0;

                N.InSynapses.ForEach(S =>
                {
                    S.Weight += S.BatchDelta;
                    S.BatchDelta = 0.0;
                });
            });
        }

        public abstract void Connect(ILayer inLayer);

        public abstract void Initialize();

        public abstract void Input(ShapedArrayImmutable<double> values);
    }
}

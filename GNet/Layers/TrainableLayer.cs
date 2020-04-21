using System;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public abstract class TrainableLayer : ILayer
    {
        public static IInitializer DefaultWeightInit { get; set; } = new Initializers.GlorotUniform();
        public static IInitializer DefaultBiasInit { get; set; } = new Initializers.Zero();

        public abstract ImmutableShapedArray<Neuron> Neurons { get; }
        public abstract Shape Shape { get; }
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public bool IsTrainable { get; set; } = true;

        protected TrainableLayer(IActivation activation, IInitializer? weightInit, IInitializer? biasInit)
        {
            Activation = activation;
            WeightInit = weightInit ?? DefaultWeightInit;
            BiasInit = biasInit ?? DefaultBiasInit;
        }

        public void Forward()
        {
            Neurons.ForEach(N => N.InVal = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal));

            ImmutableArray<double> activated = Activation.Activate(Neurons.Select(N => N.InVal));

            Neurons.ForEach((N, i) => N.OutVal = activated[i]);
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            //todo: appereneatly its correct only for regression problems? maybe?
            ImmutableArray<double> actvDers = Activation.Derivative(Neurons.Select(N => N.InVal));
            ImmutableArray<double> lossDers = loss.Derivative(targets, Neurons.Select(N => N.OutVal));
            ImmutableArray<double> grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }

        public void CalcGrads()
        {
            ImmutableArray<double> actvDers = Activation.Derivative(Neurons.Select(N => N.InVal));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
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

        public abstract void Input(ImmutableShapedArray<double> values);
    }
}

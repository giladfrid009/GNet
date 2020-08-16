using System;

namespace GNet.Layers
{
    [Serializable]
    public abstract class TrainableLayer : Layer
    {
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public bool IsTrainable { get; set; } = true;

        protected TrainableLayer(Shape shape, IActivation activation, IInitializer? weightInit, IInitializer? biasInit) : base(shape)
        {
            Activation = activation;
            WeightInit = weightInit ?? Defaults.WeightInit;
            BiasInit = biasInit ?? Defaults.BiasInit;
        }

        public sealed override void Optimize(IOptimizer optimizer)
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

        public sealed override void Update()
        {
            if (IsTrainable == false)
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

        public override void Forward(bool isTraining)
        {
            Neurons.ForEach((N, i) =>
            {
                N.InVal = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);
                N.OutVal = Activation.Activate(N.InVal);
            });
        }

        public override void CalcGrads(ILoss loss, Array<double> targets)
        {
            Neurons.ForEach((N, i) =>
            {
                N.Gradient = Activation.Derivative(N.InVal, N.OutVal) * loss.Derivative(targets[i], N.OutVal);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }

        public override void CalcGrads()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient) * Activation.Derivative(N.InVal, N.OutVal);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }
    }
}
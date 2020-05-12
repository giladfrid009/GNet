using System;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public abstract class TrainableLayer : ILayer
    {      
        public abstract ImmutableArray<Neuron> Neurons { get; }
        public Shape Shape { get; }
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public bool IsTrainable { get; set; } = true;

        protected TrainableLayer(Shape shape, IActivation activation, IInitializer? weightInit, IInitializer? biasInit)
        {
            Shape = shape;
            Activation = activation;
            WeightInit = weightInit ?? DefaultParams.WeightInit;
            BiasInit = biasInit ?? DefaultParams.BiasInit;
        }

        public void Forward(bool isTraining)
        {
            Neurons.ForEach((N, i) =>
            {
                N.InVal = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);
                N.OutVal = Activation.Activate(N.InVal);
            });
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = Activation.Derivative(N.InVal, N.OutVal) * loss.Derivative(targets[i], N.OutVal);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }

        public void CalcGrads()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient) * Activation.Derivative(N.InVal, N.OutVal);
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

        public abstract void Input(ImmutableShapedArray<double> values, bool isTraining);
    }
}

using GNet.Model;
using System;

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
        public IRegularizer? WeightReg { get; set; } = Defaults.WeightReg;
        public IRegularizer? BiasReg { get; set; } = Defaults.BiasReg;
        public IConstraint? WeightConst { get; set; } = Defaults.WeightConst;
        public IConstraint? BiasConst { get; set; } = Defaults.BiasConst;
        public bool IsTrainable { get; set; } = true;

        protected TrainableLayer(Shape shape, IActivation activation, IInitializer? weightInit, IInitializer? biasInit)
        {
            Shape = shape;
            Activation = activation;
            WeightInit = weightInit ?? Defaults.WeightInit;
            BiasInit = biasInit ?? Defaults.BiasInit;
        }

        protected void ApplyRegularizers()
        {
            if (BiasReg != null)
            {
                Neurons.ForEach(N => N.Gradient *= BiasReg.Derivative(N.Bias));
            }

            if (WeightReg != null)
            {
                Neurons.ForEach(N => N.InSynapses.ForEach(S => S.Gradient *= WeightReg.Derivative(S.Weight)));
            }
        }

        protected void ApplyConstraints()
        {
            //todo: implement
            if (BiasConst != null)
            {

            }

            if (WeightConst != null)
            {

            }
        }

        public virtual void Forward(bool isTraining)
        {
            Neurons.ForEach((N, i) =>
            {
                N.InVal = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);
                N.OutVal = Activation.Activate(N.InVal);
            });
        }

        public virtual void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
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

            ApplyRegularizers();
        }

        public virtual void CalcGrads()
        {
            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient) * Activation.Derivative(N.InVal, N.OutVal);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });

            ApplyRegularizers();
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

            ApplyConstraints();
        }

        public abstract void Connect(ILayer inLayer);

        public abstract void Initialize();

        public abstract void Input(ImmutableShapedArray<double> values, bool isTraining);
    }
}
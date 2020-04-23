using System;
using GNet.Model;

namespace GNet.Layers
{
    //todo: implement
    [Serializable]
    public class BatchNorm : ILayer
    {
        public ImmutableShapedArray<Neuron> Neurons { get; }
        public Shape Shape { get; }
        public double Momentum { get; }
        public double Beta { get; private set; }
        public double Gamma { get; private set; }
        
        private double movingAvg;
        private double movingVar;

        public BatchNorm(Shape shape, double momentum = 0.99, double beta = 0.0, double gamma = 1.0)
        {
            Shape = shape;
            Momentum = momentum;
            Beta = beta;
            Gamma = gamma;

            Neurons = new ImmutableShapedArray<Neuron>(shape, () => new Neuron());
        }

        public void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
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

        public void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            Neurons.ForEach((N, i) => N.InVal = values[i]);

            Normalize(isTraining);
        }

        public void Forward(bool isTraining)
        {
            Neurons.ForEach((N, i) => N.InVal = N.InSynapses[0].InNeuron.OutVal);

            Normalize(isTraining);
        }

        private void Normalize(bool isTraining)
        {
            if (isTraining)
            {
                ImmutableArray<double> values = Neurons.Select(N => N.InVal);

                double avg = values.Avarage();

                double var = values.Sum(X => (X - avg) * (X - avg));

                movingAvg = Momentum * movingAvg + (1.0 - Momentum) * avg;

                movingVar = Momentum * movingVar + (1.0 - Momentum) * var;
            }

            Neurons.ForEach((N, i) => N.OutVal = Gamma * (N.InVal - movingAvg) / Math.Sqrt(movingVar + double.Epsilon) + Beta);
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            //todo: implement
        }

        public void CalcGrads()
        {
            //todo: implement
        }

        public void Optimize(IOptimizer optimizer)
        {
        }

        public void Update()
        {
            //todo: reset batch params? or dont?
        }
    }
}
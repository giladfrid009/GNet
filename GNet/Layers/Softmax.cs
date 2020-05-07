using GNet.Model;
using static System.Math;

namespace GNet.Layers
{
    public class Softmax : ILayer
    {
        public ImmutableShapedArray<Neuron> Neurons { get; }
        public Shape Shape { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public bool IsTrainable { get; set; } = true;

        public Softmax(Shape shape, IInitializer? weightInit = null, IInitializer? biasInit = null)
        {
            Shape = shape;
            WeightInit = weightInit ?? TrainableLayer.DefaultWeightInit;
            BiasInit = biasInit ?? TrainableLayer.DefaultBiasInit;

            Neurons = new ImmutableShapedArray<Neuron>(shape, () => new Neuron());      
        }

        //todo: same as dense layer
        public void Connect(ILayer inLayer)
        {
            Neurons.ForEach(N => N.InSynapses = inLayer.Neurons.Select(inN => new Synapse(inN, N)));

            inLayer.Neurons.ForEach((inN, i) => inN.OutSynapses = Neurons.Select(outN => outN.InSynapses[i]));
        }

        //todo: same as dense layer
        public void Initialize()
        {
            int inLength = Neurons[0].InSynapses.Length;
            int outLength = Shape.Volume;

            Neurons.ForEach(N =>
            {
                N.Bias = BiasInit.Initialize(inLength, outLength);
                N.InSynapses.ForEach(S => S.Weight = WeightInit.Initialize(inLength, outLength));
            });
        }

        public void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            double eSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.InVal = values[i];
                N.OutVal = Exp(N.InVal);
                eSum += N.OutVal;
            });

            Neurons.ForEach(N => N.OutVal /= eSum);
        }

        public void Forward(bool isTraining)
        {
            double eSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.InVal = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);
                N.OutVal = Exp(N.InVal);
                eSum += N.OutVal;
            });

            Neurons.ForEach(N => N.OutVal /= eSum);
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            double gSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = loss.Derivative(targets[i], N.OutVal);
                gSum += N.Gradient * N.OutVal;
            });

            Neurons.ForEach(N =>
            {
                N.Gradient = N.OutVal * (N.Gradient - gSum);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }

        public void CalcGrads()
        {
            double gSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient);
                gSum += N.Gradient * N.OutVal;
            });

            Neurons.ForEach(N =>
            {
                N.Gradient = N.OutVal * (N.Gradient - gSum);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }

        //todo: same as trainable layer
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

        //todo: same as trainable layer
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
        }
    }
}

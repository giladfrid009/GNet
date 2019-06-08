using static System.Math;
using GNet.Extensions;

namespace GNet
{
    public interface IOptimizer : ICloneable<IOptimizer>
    {
        void Optimize(Neuron[] neurons);
    }
}
namespace GNet.Optimizers
{
    public class Default : IOptimizer
    {
        public double LearningRate { get; }

        public Default(double learningRate)
        {
            LearningRate = learningRate;
        }        

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.BatchBias += -LearningRate * N.Gradient;

                N.InSynapses.ForEach(S =>
                {
                    S.BatchWeight += -LearningRate * S.Gradient;
                });
            });
        }

        public IOptimizer Clone() => new Default(LearningRate);
    }

    public class Momentum : IOptimizer
    {
        public double LearningRate { get; }
        public double MomentumValue { get; }

        public Momentum(double learningRate, double momentum)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                var oldDelta = N.SavedValue;
                N.SavedValue = -LearningRate * N.Gradient;
                N.BatchBias += N.SavedValue + MomentumValue * oldDelta;

                N.InSynapses.ForEach(S =>
                {
                    oldDelta = S.SavedValue;
                    S.SavedValue = -LearningRate * S.Gradient;
                    S.BatchWeight += S.SavedValue + MomentumValue * oldDelta;
                });
            });
        }

        public IOptimizer Clone() => new Momentum(LearningRate, MomentumValue);
    }

    // todo: implement
    public class NestrovMomentum : IOptimizer
    {
        public double LearningRate { get; }
        public double MomentumValue { get; }

        public NestrovMomentum(double learningRate, double momentum)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                
            });
        }        

        public IOptimizer Clone() => new NestrovMomentum(LearningRate, MomentumValue);
    }

    public class AdaGrad : IOptimizer
    {
        public double LearningRate { get; }
        public double Epsilon { get; } = 1e-8;

        public AdaGrad(double learningRate)
        {
            LearningRate = learningRate;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.SavedValue += N.Gradient * N.Gradient;
                N.BatchBias += -LearningRate * N.Gradient / Sqrt(N.SavedValue + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.SavedValue = S.SavedValue + N.Gradient * S.InNeuron.ActivatedValue * N.Gradient * S.InNeuron.ActivatedValue;
                    S.BatchWeight += -LearningRate * N.Gradient * S.InNeuron.ActivatedValue / Sqrt(N.SavedValue + Epsilon);
                });
            });
        }


        public IOptimizer Clone() => new AdaGrad(LearningRate);
    }

    public class AdaGradWindow : IOptimizer
    {
        public double LearningRate { get; }
        public double Epsilon { get; } = 1e-8;
        public double Rho { get; } = 0.95;

        public AdaGradWindow(double learningRate)
        {
            LearningRate = learningRate;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.SavedValue = Rho * N.SavedValue + (1.0 - Rho) * N.Gradient * N.Gradient;
                N.BatchBias += -LearningRate * N.Gradient / Sqrt(N.SavedValue + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.SavedValue = Rho * S.SavedValue + (1.0 - Rho) * N.Gradient * S.InNeuron.ActivatedValue * N.Gradient * S.InNeuron.ActivatedValue;
                    S.BatchWeight += -LearningRate * N.Gradient * S.InNeuron.ActivatedValue / Sqrt(N.SavedValue + Epsilon);
                });
            });
        }


        public IOptimizer Clone() => new AdaGrad(LearningRate);
    }

    // todo: implement
    public class AdaDelta
    {

    }

    // todo: implement
    public class RMSProp
    {

    }

    // todo: implement
    public class Adam
    {

    }

    // todo: implement
    public class AdaMax
    {

    }
}

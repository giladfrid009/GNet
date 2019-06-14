using System;
using GNet.Extensions;
using static System.Math;

namespace GNet
{
    public interface IOptimizer : ICloneable<IOptimizer>
    {
        void Optimize(Neuron[] neurons);
    }
}

// todo: test all algs.

namespace GNet.Optimizers
{
    public class Default : IOptimizer
    {
        public enum DecayTypes { Step, Exponential, TimeBased }

        public double LearningRate { get; private set; }
        public double Decay { get; }
        public DecayTypes DecayType { get; }

        private int iteration = 0;
        private readonly Func<double, double> decayFunc;

        public Default(double learningRate, double decay = 0, DecayTypes decayType = DecayTypes.Step)
        {
            LearningRate = learningRate;
            Decay = decay;
            DecayType = decayType;

            switch (DecayType)
            {
                case DecayTypes.Step: decayFunc = (LR) => LR - Decay; break;

                case DecayTypes.Exponential: decayFunc = (LR) => LR * Exp(-Decay * iteration); break;

                case DecayTypes.TimeBased: decayFunc = (LR) => LR / (1 + Decay * iteration); break;
            }
        }

        public void Optimize(Neuron[] neurons)
        {
            LearningRate = decayFunc(LearningRate);

            neurons.ForEach(N =>
            {
                N.BatchBias += -LearningRate * N.Gradient;

                N.InSynapses.ForEach(S =>
                {
                    S.BatchWeight += -LearningRate * S.Gradient;
                });
            });

            iteration++;
        }

        public IOptimizer Clone() => new Default(LearningRate, Decay, DecayType);
    }

    public class Momentum : IOptimizer
    {
        public enum DecayTypes { Step, Exponential, TimeBased }

        public double LearningRate { get; private set; }
        public double MomentumValue { get; }
        public double Decay { get; }
        public DecayTypes DecayType { get; }

        private int iteration = 0;
        private readonly Func<double, double> decayFunc;

        public Momentum(double learningRate, double momentum = 0.9, double decay = 0, DecayTypes decayType = DecayTypes.Step)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay;
            DecayType = decayType;

            switch (DecayType)
            {
                case DecayTypes.Step: decayFunc = (LR) => LR - Decay; break;

                case DecayTypes.Exponential: decayFunc = (LR) => LR * Exp(-Decay * iteration); break;

                case DecayTypes.TimeBased: decayFunc = (LR) => LR / (1 + Decay * iteration); break;
            }
        }

        public void Optimize(Neuron[] neurons)
        {
            LearningRate = decayFunc(LearningRate);

            neurons.ForEach(N =>
            {
                N.Cache1 = -LearningRate * N.Gradient + MomentumValue * N.Cache1;
                N.BatchBias += N.Cache1;

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = -LearningRate * S.Gradient + MomentumValue * S.Cache1;
                    S.BatchWeight += S.Cache1;
                });
            });

            iteration++;
        }

        public IOptimizer Clone() => new Momentum(LearningRate, MomentumValue, Decay, DecayType);
    }

    public class NestrovMomentum : IOptimizer
    {
        public enum DecayTypes { Step, Exponential, TimeBased }

        public double LearningRate { get; private set; }
        public double MomentumValue { get; }
        public double Decay { get; }
        public DecayTypes DecayType { get; }

        private int iteration = 0;
        private readonly Func<double, double> decayFunc;

        public NestrovMomentum(double learningRate, double momentum = 0.9, double decay = 0, DecayTypes decayType = DecayTypes.Step)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay;
            DecayType = decayType;

            switch (DecayType)
            {
                case DecayTypes.Step: decayFunc = (LR) => LR - Decay; break;

                case DecayTypes.Exponential: decayFunc = (LR) => LR * Exp(-Decay * iteration); break;

                case DecayTypes.TimeBased: decayFunc = (LR) => LR / (1 + Decay * iteration); break;
            }
        }

        public void Optimize(Neuron[] neurons)
        {
            LearningRate = decayFunc(LearningRate);

            neurons.ForEach(N =>
            {
                var oldDelta = N.Cache1;
                N.Cache1 = -LearningRate * N.Gradient + MomentumValue * N.Cache1;
                N.BatchBias += (1 + MomentumValue) * N.Cache1 - MomentumValue * oldDelta;

                N.InSynapses.ForEach(S =>
                {
                    oldDelta = S.Cache1;
                    S.Cache1 = -LearningRate * S.Gradient + MomentumValue * S.Cache1;
                    S.BatchWeight += (1 + MomentumValue) * S.Cache1 - MomentumValue * oldDelta;
                });
            });

            iteration++;
        }

        public IOptimizer Clone() => new NestrovMomentum(LearningRate, MomentumValue, Decay, DecayType);
    }

    public class AdaGrad : IOptimizer
    {
        public double LearningRate { get; }
        public double Epsilon { get; } = 1e-8;

        public AdaGrad(double learningRate = 0.01)
        {
            LearningRate = learningRate;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.Cache1 += N.Gradient * N.Gradient;
                N.BatchBias += -LearningRate * N.Gradient / Sqrt(N.Cache1 + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 += S.Gradient * S.Gradient;
                    S.BatchWeight += -LearningRate * S.Gradient / Sqrt(S.Cache1 + Epsilon);
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

        public AdaGradWindow(double learningRate = 0.01, double rho = 0.95)
        {
            LearningRate = learningRate;
            Rho = rho;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.Cache1 = Rho * N.Cache1 + (1 - Rho) * N.Gradient * N.Gradient;
                N.BatchBias += -LearningRate * N.Gradient / Sqrt(N.Cache1 + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Rho * S.Cache1 + (1 - Rho) * S.Gradient * S.Gradient;
                    S.BatchWeight += -LearningRate * S.Gradient / Sqrt(S.Cache1 + Epsilon);
                });
            });
        }


        public IOptimizer Clone() => new AdaGradWindow(LearningRate, Rho);
    }

    public class AdaDelta : IOptimizer
    {
        public double Epsilon { get; } = 1e-8;
        public double Rho { get; } = 0.95;

        public AdaDelta(double rho = 0.95)
        {
            Rho = rho;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.Cache1 = Rho * N.Cache1 + (1 - Rho) * N.Gradient * N.Gradient;
                var delta = -Sqrt(N.Cache2 + Epsilon) * N.Gradient / Sqrt(N.Cache1 + Epsilon);
                N.Cache2 = Rho * N.Cache2 + (1 - Rho) * delta * delta;
                N.BatchBias += delta;

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Rho * N.Cache1 + (1 - Rho) * S.Gradient * S.Gradient;
                    delta = -Sqrt(S.Cache2 + Epsilon) * S.Gradient / Sqrt(S.Cache1 + Epsilon);
                    S.Cache2 = Rho * S.Cache2 + (1 - Rho) * delta * delta;
                    S.BatchWeight += delta;
                });
            });
        }


        public IOptimizer Clone() => new AdaDelta(Rho);
    }

    public class RMSProp : IOptimizer
    {
        public double LearningRate { get; }
        public double Decay { get; }
        public double Epsilon { get; } = 1e-8;

        public RMSProp(double learningRate, double decay = 0.9)
        {
            LearningRate = learningRate;
            Decay = decay;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.Cache1 = Decay * N.Cache1 + (1 - Decay) * N.Gradient * N.Gradient;
                N.BatchBias += -LearningRate * N.Gradient / (Sqrt(N.Cache1) + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Decay * S.Cache1 + (1 - Decay) * S.Gradient * S.Gradient;
                    S.BatchWeight += -LearningRate * S.Gradient / (Sqrt(S.Cache1) + Epsilon);
                });
            });
        }

        public IOptimizer Clone() => new RMSProp(LearningRate, Decay);
    }

    public class Adam : IOptimizer
    {
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; } = 1e-8;

        public Adam(double learningRate = 0.001, double beta1 = 0.9, double beta2 = 0.999)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
        }

        public void Optimize(Neuron[] neurons)
        {
            neurons.ForEach(N =>
            {
                N.Cache1 = Beta1 * N.Cache1 + (1 - Beta1) * N.Gradient;
                N.Cache2 = Beta2 * N.Cache2 + (1 - Beta2) * N.Gradient * N.Gradient;
                N.BatchBias += -LearningRate * N.Cache1 / (Sqrt(N.Cache2) + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Beta1 * S.Cache1 + (1 - Beta1) * S.Gradient;
                    S.Cache2 = Beta2 * S.Cache2 + (1 - Beta2) * S.Gradient * S.Gradient;
                    S.BatchWeight += -LearningRate * S.Cache1 / (Sqrt(S.Cache2) + Epsilon);
                });
            });
        }

        public IOptimizer Clone() => new Adam(LearningRate, Beta1, Beta2);
    }

    // todo: implement
    public class AdaMax
    {

    }
}

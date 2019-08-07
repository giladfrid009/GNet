using GNet.Extensions.Generic;
using static System.Math;

namespace GNet
{
    public interface IOptimizer : ICloneable<IOptimizer>
    {
        void Optimize(Neuron[] neurons, int epoch);
    }
}

namespace GNet.Optimizers
{
    public class Default : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }

        public Default(double learningRate = 0.01, IDecay decay = null)
        {
            LearningRate = learningRate;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            neurons.ForEach(N =>
            {
                N.BatchBias += -lr * N.Gradient;

                N.InSynapses.ForEach(S =>
                {
                    S.BatchWeight += -lr * S.Gradient;
                });
            });
        }

        public IOptimizer Clone()
        {
            return new Default(LearningRate, Decay);
        }
    }

    public class Momentum : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double MomentumValue { get; }

        public Momentum(double learningRate = 0.01, double momentum = 0.9, IDecay decay = null)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            neurons.ForEach(N =>
            {
                N.Cache1 = -lr * N.Gradient + MomentumValue * N.Cache1;
                N.BatchBias += N.Cache1;

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = -lr * S.Gradient + MomentumValue * S.Cache1;
                    S.BatchWeight += S.Cache1;
                });
            });
        }

        public IOptimizer Clone()
        {
            return new Momentum(LearningRate, MomentumValue, Decay);
        }
    }

    public class NestrovMomentum : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double MomentumValue { get; }

        public NestrovMomentum(double learningRate = 0.01, double momentum = 0.9, IDecay decay = null)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            neurons.ForEach(N =>
            {
                double oldDelta = N.Cache1;
                N.Cache1 = -lr * N.Gradient + MomentumValue * N.Cache1;
                N.BatchBias += (1.0 + MomentumValue) * N.Cache1 - MomentumValue * oldDelta;

                N.InSynapses.ForEach(S =>
                {
                    oldDelta = S.Cache1;
                    S.Cache1 = -lr * S.Gradient + MomentumValue * S.Cache1;
                    S.BatchWeight += (1.0 + MomentumValue) * S.Cache1 - MomentumValue * oldDelta;
                });
            });

        }

        public IOptimizer Clone()
        {
            return new NestrovMomentum(LearningRate, MomentumValue, Decay);
        }
    }

    public class AdaGrad : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Epsilon { get; }

        public AdaGrad(double learningRate = 0.01, double epsilon = 1e-8, IDecay decay = null)
        {
            LearningRate = learningRate;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            neurons.ForEach(N =>
            {
                N.Cache1 += N.Gradient * N.Gradient;
                N.BatchBias += -lr * N.Gradient / (Sqrt(N.Cache1) + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 += S.Gradient * S.Gradient;
                    S.BatchWeight += -lr * S.Gradient / (Sqrt(S.Cache1) + Epsilon);
                });
            });
        }


        public IOptimizer Clone()
        {
            return new AdaGrad(LearningRate, Epsilon, Decay);
        }
    }

    public class AdaGradWindow : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }
        public double Epsilon { get; }

        public AdaGradWindow(double learningRate = 0.01, double rho = 0.95, double epsilon = 1e-8, IDecay decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            neurons.ForEach(N =>
            {
                N.Cache1 = Rho * N.Cache1 + (1.0 - Rho) * N.Gradient * N.Gradient;
                N.BatchBias += -lr * N.Gradient / Sqrt(N.Cache1 + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Rho * S.Cache1 + (1.0 - Rho) * S.Gradient * S.Gradient;
                    S.BatchWeight += -lr * S.Gradient / Sqrt(S.Cache1 + Epsilon);
                });
            });
        }


        public IOptimizer Clone()
        {
            return new AdaGradWindow(LearningRate, Rho, Epsilon, Decay);
        }
    }

    public class AdaDelta : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }
        public double Epsilon { get; }

        public AdaDelta(double learningRate = 1.0, double rho = 0.95, double epsilon = 1e-8, IDecay decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            neurons.ForEach(N =>
            {
                N.Cache1 = Rho * N.Cache1 + (1.0 - Rho) * N.Gradient * N.Gradient;
                double delta = -Sqrt(N.Cache2 + Epsilon) * N.Gradient / Sqrt(N.Cache1 + Epsilon);
                N.Cache2 = Rho * N.Cache2 + (1.0 - Rho) * delta * delta;
                N.BatchBias += delta;

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Rho * S.Cache1 + (1.0 - Rho) * S.Gradient * S.Gradient;
                    delta = -Sqrt(S.Cache2 + Epsilon) * S.Gradient / Sqrt(S.Cache1 + Epsilon);
                    S.Cache2 = Rho * S.Cache2 + (1.0 - Rho) * delta * delta;
                    S.BatchWeight += lr * delta;
                });
            });
        }


        public IOptimizer Clone()
        {
            return new AdaDelta(LearningRate, Rho, Epsilon, Decay);
        }
    }

    public class RMSProp : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }
        public double Epsilon { get; }

        public RMSProp(double learningRate = 0.001, double rho = 0.9, double epsilon = 1e-8, IDecay decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            neurons.ForEach(N =>
            {
                N.Cache1 = Rho * N.Cache1 + (1.0 - Rho) * N.Gradient * N.Gradient;
                N.BatchBias += -lr * N.Gradient / (Sqrt(N.Cache1) + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Rho * S.Cache1 + (1.0 - Rho) * S.Gradient * S.Gradient;
                    S.BatchWeight += -lr * S.Gradient / (Sqrt(S.Cache1) + Epsilon);
                });
            });
        }

        public IOptimizer Clone()
        {
            return new RMSProp(LearningRate, Rho, Epsilon, Decay);
        }
    }

    public class Adam : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        public Adam(double learningRate = 0.001, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-8, IDecay decay = null)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            double val1 = 1.0 - Pow(Beta1, epoch + 1.0);
            double val2 = 1.0 - Pow(Beta2, epoch + 1.0);

            neurons.ForEach(N =>
            {
                N.Cache1 = Beta1 * N.Cache1 + (1.0 - Beta1) * N.Gradient;
                N.Cache2 = Beta2 * N.Cache2 + (1.0 - Beta2) * N.Gradient * N.Gradient;
                double corr1 = N.Cache1 / val1;
                double corr2 = N.Cache2 / val2;
                N.BatchBias += -lr * corr1 / (Sqrt(corr2) + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Beta1 * S.Cache1 + (1.0 - Beta1) * S.Gradient;
                    S.Cache2 = Beta2 * S.Cache2 + (1.0 - Beta2) * S.Gradient * S.Gradient;
                    corr1 = S.Cache1 / val1;
                    corr2 = S.Cache2 / val2;
                    S.BatchWeight += -lr * corr1 / (Sqrt(corr2) + Epsilon);
                });
            });
        }

        public IOptimizer Clone()
        {
            return new Adam(LearningRate, Beta1, Beta2, Epsilon, Decay);
        }
    }

    public class AdaMax : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        public AdaMax(double learningRate = 0.002, double beta1 = 0.9, double beta2 = 0.999, IDecay decay = null)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            double val1 = 1.0 - Pow(Beta1, epoch + 1.0);

            neurons.ForEach(N =>
            {
                N.Cache1 = Beta1 * N.Cache1 + (1.0 - Beta1) * N.Gradient;
                N.Cache2 = Max(Beta2 * N.Cache2, Abs(N.Gradient));
                double corr1 = N.Cache1 / val1;
                N.BatchBias += -lr * corr1 / (N.Cache2 + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Beta1 * S.Cache1 + (1.0 - Beta1) * S.Gradient;
                    S.Cache2 = Max(Beta2 * S.Cache2, Abs(S.Gradient));
                    corr1 = S.Cache1 / val1;
                    S.BatchWeight += -lr * corr1 / S.Cache2;
                });
            });
        }

        public IOptimizer Clone()
        {
            return new AdaMax(LearningRate, Beta1, Beta2, Decay);
        }
    }

    public class Nadam : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        public Nadam(double learningRate = 0.002, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-8, IDecay decay = null)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Neuron[] neurons, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            double val1 = 1.0 - Pow(Beta1, epoch + 1.0);
            double val2 = 1.0 - Pow(Beta2, epoch + 1.0);

            neurons.ForEach(N =>
            {
                N.Cache1 = Beta1 * N.Cache1 + (1.0 - Beta1) * N.Gradient;
                N.Cache2 = Beta2 * N.Cache2 + (1.0 - Beta2) * N.Gradient * N.Gradient;
                double corr1 = N.Cache1 / val1 + (1.0 - Beta1) * N.Gradient / val1;
                double corr2 = N.Cache2 / val2;
                N.BatchBias += -lr * corr1 / (Sqrt(corr2) + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Beta1 * S.Cache1 + (1.0 - Beta1) * S.Gradient;
                    S.Cache2 = Beta2 * S.Cache2 + (1.0 - Beta2) * S.Gradient * S.Gradient;
                    corr1 = S.Cache1 / val1 + (1.0 - Beta1) * S.Gradient / val1;
                    corr2 = S.Cache2 / val2;
                    S.BatchWeight += -lr * corr1 / (Sqrt(corr2) + Epsilon);
                });
            });
        }

        public IOptimizer Clone()
        {
            return new Nadam(LearningRate, Beta1, Beta2, Epsilon, Decay);
        }
    }
}

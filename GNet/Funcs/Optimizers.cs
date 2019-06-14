using GNet.Extensions;
using static System.Math;

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

        public Default(double learningRate = 0.01)
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

        public Momentum(double learningRate = 0.01, double momentum = 0.9)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
        }

        public void Optimize(Neuron[] neurons)
        {
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
        }

        public IOptimizer Clone() => new Momentum(LearningRate, MomentumValue);
    }


    public class NestrovMomentum : IOptimizer
    {
        public double LearningRate { get; }
        public double MomentumValue { get; }

        public NestrovMomentum(double learningRate = 0.01, double momentum = 0.9)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
        }

        public void Optimize(Neuron[] neurons)
        {
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

        }

        public IOptimizer Clone() => new NestrovMomentum(LearningRate, MomentumValue);
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
                    S.Cache1 = Rho * S.Cache1 + (1 - Rho) * S.Gradient * S.Gradient;
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

        public RMSProp(double learningRate = 0.001, double decay = 0.9)
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

        private readonly int batchSize;
        private int index = 0;
        private int epoch = 0;

        public Adam(int batchSize, double learningRate = 0.001, double beta1 = 0.9, double beta2 = 0.999)
        {
            this.batchSize = batchSize;
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
        }

        public void Optimize(Neuron[] neurons)
        {
            if (index % batchSize == 0)
                epoch++;

            neurons.ForEach(N =>
            {
                N.Cache1 = Beta1 * N.Cache1 + (1 - Beta1) * N.Gradient;
                N.Cache2 = Beta2 * N.Cache2 + (1 - Beta2) * N.Gradient * N.Gradient;
                var corr1 = N.Cache1 / (1 - Pow(Beta1, epoch + 1));
                var corr2 = N.Cache2 / (1 - Pow(Beta2, epoch + 1));
                N.BatchBias += -LearningRate * corr1 / (Sqrt(corr2) + Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Beta1 * S.Cache1 + (1 - Beta1) * S.Gradient;
                    S.Cache2 = Beta2 * S.Cache2 + (1 - Beta2) * S.Gradient * S.Gradient;
                    corr1 = S.Cache1 / (1 - Pow(Beta1, epoch + 1));
                    corr2 = S.Cache2 / (1 - Pow(Beta2, epoch + 1));
                    S.BatchWeight += -LearningRate * corr1 / (Sqrt(corr2) + Epsilon);
                });
            });

            index++;
        }

        public IOptimizer Clone() => new Adam(batchSize, LearningRate, Beta1, Beta2);
    }    
}

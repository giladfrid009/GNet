using GNet.Extensions.Generic;
using static System.Math;

namespace GNet.Optimizers
{
    public class AdaMax : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        public AdaMax(double learningRate = 0.002, double beta1 = 0.9, double beta2 = 0.999, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Dense layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            double val1 = 1.0 - Pow(Beta1, epoch + 1.0);

            layer.Neurons.ForEach(N =>
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
}

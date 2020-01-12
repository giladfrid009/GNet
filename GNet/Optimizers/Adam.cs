﻿using GNet.Extensions.IArray;
using static System.Math;

namespace GNet.Optimizers
{
    public class Adam : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        public Adam(double learningRate = 0.001, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-8, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(ILayer layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            double val1 = 1.0 - Pow(Beta1, epoch + 1.0);
            double val2 = 1.0 - Pow(Beta2, epoch + 1.0);

            layer.InNeurons.ForEach(N =>
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
}

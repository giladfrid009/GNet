using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using static System.Math;

namespace GNet.Optimizers
{
    public class AdaDelta : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }
        public double Epsilon { get; }

        public AdaDelta(double learningRate = 1.0, double rho = 0.95, double epsilon = 1e-8, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Epsilon = epsilon;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Dense layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            layer.Neurons.ForEach(N =>
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
}

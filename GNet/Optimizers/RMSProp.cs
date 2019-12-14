using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using static System.Math;

namespace GNet.Optimizers
{
    public class RMSProp : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }
        public double Epsilon { get; }

        public RMSProp(double learningRate = 0.001, double rho = 0.9, double epsilon = 1e-8, IDecay? decay = null)
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
}

using GNet.Extensions.IArray;

namespace GNet.Optimizers
{
    public class NestrovMomentum : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double MomentumValue { get; }

        public NestrovMomentum(double learningRate = 0.01, double momentum = 0.9, IDecay? decay = null)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(ILayer layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            layer.InNeurons.ForEach(N =>
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
}

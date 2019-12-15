using GNet.Extensions.Array;
using GNet.Extensions.IArray;
namespace GNet.Optimizers
{
    public class Momentum : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double MomentumValue { get; }

        public Momentum(double learningRate = 0.01, double momentum = 0.9, IDecay? decay = null)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(Dense layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            layer.Neurons.ForEach(N =>
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
}

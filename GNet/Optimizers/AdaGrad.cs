using static System.Math;

namespace GNet.Optimizers
{
    public class AdaGrad : IOptimizer
    {
        public IDecay Decay { get; }
        public double Epsilon { get; }
        public double LearningRate { get; }

        public AdaGrad(double learningRate = 0.01, double epsilon = 1e-8, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Epsilon = epsilon;
            Decay = decay ?? new Decays.None();
        }

        public void Optimize(ILayer layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            layer.Neurons.ForEach(N =>
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
    }
}
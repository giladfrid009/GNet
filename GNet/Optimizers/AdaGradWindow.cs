using static System.Math;

namespace GNet.Optimizers
{
    public class AdaGradWindow : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }

        public AdaGradWindow(double learningRate = 0.01, double rho = 0.95, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Decay = decay ?? new Decays.None();
        }

        public void Optimize(ILayer layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            layer.Neurons.ForEach(N =>
            {
                N.Cache1 = Rho * N.Cache1 + (1.0 - Rho) * N.Gradient * N.Gradient;
                N.BatchBias += -lr * N.Gradient / Sqrt(N.Cache1 + double.Epsilon);

                N.InSynapses.ForEach(S =>
                {
                    S.Cache1 = Rho * S.Cache1 + (1.0 - Rho) * S.Gradient * S.Gradient;
                    S.BatchWeight += -lr * S.Gradient / Sqrt(S.Cache1 + double.Epsilon);
                });
            });
        }
    }
}
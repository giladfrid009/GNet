namespace GNet.Optimizers
{
    public class Momentum : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double MomentumValue { get; }

        private double EpochLr;

        public Momentum(double learningRate = 0.01, double momentum = 0.9, IDecay? decay = null)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            EpochLr = Decay.Compute(LearningRate, epoch);
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 = -EpochLr * O.Gradient + MomentumValue * O.Cache1;
            return O.Cache1;
        }
    }
}
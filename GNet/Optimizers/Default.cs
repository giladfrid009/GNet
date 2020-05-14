namespace GNet.Optimizers
{
    public class Default : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }

        private double epochLr;

        public Default(double learningRate = 0.01, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay.Compute(LearningRate, epoch);
        }

        public double Optimize(IOptimizable O)
        {
            return -epochLr * O.Gradient;
        }
    }
}
namespace GNet.Optimizers
{
    public class NesterovMomentum : IOptimizer
    {
        public IDecay? Decay { get; }
        public double LearningRate { get; }
        public double Momentum { get; }
        public double Epsilon { get; }

        private double epochLr;

        public NesterovMomentum(double learningRate = 0.01, double momentum = 0.9, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Momentum = momentum;
            Decay = decay;
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay?.Compute(LearningRate, epoch) ?? LearningRate;
        }

        public double Optimize(TrainableObj O)
        {
            double oldDelta = O.Cache1;
            O.Cache1 = -epochLr * O.Gradient + Momentum * O.Cache1;
            return O.Cache1 + Momentum * (O.Cache1 - oldDelta);
        }
    }
}
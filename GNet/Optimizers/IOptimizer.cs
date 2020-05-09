using GNet.Optimizers;

namespace GNet
{
    //todo: view old optimizer from github and check the epsilon for each one.
    public interface IOptimizer
    {
        IDecay Decay { get; }
        double LearningRate { get; }

        public void UpdateParams(int epoch);

        double Optimize(IOptimizable obj);
    }
}
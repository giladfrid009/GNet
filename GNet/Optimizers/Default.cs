using GNet.Extensions.IArray;

namespace GNet.Optimizers
{
    public class Default : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }

        public Default(double learningRate = 0.01, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Decay = decay?.Clone() ?? new Decays.None();
        }

        public void Optimize(ILayer layer, int epoch)
        {
            double lr = Decay.Compute(LearningRate, epoch);

            layer.InNeurons.ForEach(N =>
            {
                N.BatchBias += -lr * N.Gradient;

                N.InSynapses.ForEach(S => S.BatchWeight += -lr * S.Gradient);
            });
        }

        public IOptimizer Clone()
        {
            return new Default(LearningRate, Decay);
        }
    }
}

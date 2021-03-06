﻿namespace GNet.Optimizers
{
    public class Default : IOptimizer
    {
        public IDecay? Decay { get; }
        public double LearningRate { get; }

        private double epochLr;

        public Default(double learningRate = 0.01, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Decay = decay;
        }

        public void UpdateEpoch(int epoch)
        {
            epochLr = Decay?.Compute(LearningRate, epoch) ?? LearningRate;
        }

        public double Optimize(TrainableObj O)
        {
            return -epochLr * O.Gradient;
        }
    }
}
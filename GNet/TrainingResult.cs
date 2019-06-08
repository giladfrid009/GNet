using System;
using System.Collections.Generic;
using System.Text;

namespace GNet
{
    public class TrainingResult
    {
        public int EpochesCompleted { get; }
        public double FinalError { get; }
        public TimeSpan TrainingTime { get; }

        public TrainingResult(int epoches, double error, TimeSpan time)
        {
            EpochesCompleted = epoches;
            FinalError = error;
            TrainingTime = time;
        }
    }
}

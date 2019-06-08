using System;

namespace GNet
{
    public class TrainingResult
    {
        public int EpochesCompleted { get; }
        public double FinalError { get; }
        public TimeSpan ExecutionTime { get; }

        public TrainingResult(int epochesCompleted, double finalError, TimeSpan executionTime)
        {
            EpochesCompleted = epochesCompleted;
            FinalError = finalError;
            ExecutionTime = executionTime;
        }

        public void Print()
        {
            Console.WriteLine(" ---- Training Session Result ----");
            Console.WriteLine(" Epoches Completed: {0}", EpochesCompleted);
            Console.WriteLine(" Final Error:       {0}", FinalError);
            Console.WriteLine(" Execution Time:    {0}", ExecutionTime);
        }
    }
}

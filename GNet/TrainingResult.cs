using System;

namespace GNet
{
    public class TrainingResult
    {
        public TimeSpan ExecutionTime { get; }
        public double InitialError { get; }
        public double FinalError { get; }
        public int EpochesCompleted { get; }

        public TrainingResult(int epochesCompleted, double initialError, double finalError, TimeSpan executionTime)
        {
            EpochesCompleted = epochesCompleted;
            InitialError = initialError;
            FinalError = finalError;
            ExecutionTime = executionTime;
        }

        public void Print()
        {
            Console.WriteLine(" ---- Training Session Result ----");
            Console.WriteLine(" Execution Time:    {0}", ExecutionTime);
            Console.WriteLine(" Epoches Completed: {0}", EpochesCompleted);
            Console.WriteLine(" Initial Error:     {0}", InitialError);
            Console.WriteLine(" Final Error:       {0}", FinalError);
        }
    }
}

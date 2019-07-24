using System;

namespace GNet
{
    [Serializable]
    public class TrainingResult
    {
        public TimeSpan ExecutionTime { get; }
        public double InitialError { get; }
        public double FinalError { get; }
        public double ValidationError { get; }
        public int EpochesCompleted { get; }

        public TrainingResult(TimeSpan executionTime, int epochesCompleted, double initialError, double finalError, double validationError)
        {
            ExecutionTime = executionTime;
            InitialError = initialError;
            FinalError = finalError;
            ValidationError = validationError;
            EpochesCompleted = epochesCompleted;
        }

        public void Print()
        {
            Console.WriteLine(" ---- Training Session Result ----");
            Console.WriteLine(" Execution Time:    {0}", ExecutionTime);
            Console.WriteLine(" Epoches Completed: {0}", EpochesCompleted);
            Console.WriteLine(" Initial Error:     {0}", InitialError);
            Console.WriteLine(" Final Error:       {0}", FinalError);
            Console.WriteLine(" Validation Error:  {0}", ValidationError);
        }
    }
}

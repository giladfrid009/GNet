using System;
using System.Diagnostics;

namespace GNet.Utils
{
    public static class Benchmark
    {
        public static TimeSpan Time(Action action, long iterations = 1)
        {
            var sw = new Stopwatch();
            sw.Start();

            for (long i = 0; i < iterations; i++)
            {
                action();
            }

            return sw.Elapsed;
        }

        public static TimeSpan BatchTime(Func<INetwork> netCreator, Action<INetwork> netTrainer, int iterations = 1)
        {
            var sw = new Stopwatch();
            int nBatches = 0;

            for (int i = 0; i < iterations; i++)
            {
                INetwork N = netCreator();

                N.OnStart += OnStart;
                N.OnFinish += OnFinish;
                N.OnBatch += OnBatch;

                netTrainer(N);

                N.OnStart -= OnStart;
                N.OnFinish -= OnFinish;
                N.OnBatch -= OnBatch;
            }

            void OnStart(double error)
            {
                sw.Start();
            }

            void OnBatch(int batch)
            {
                nBatches++;
            }

            void OnFinish(int epoch, double error)
            {
                sw.Stop();
            }

            return sw.Elapsed / nBatches;
        }
    }
}
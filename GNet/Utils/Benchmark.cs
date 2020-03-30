using System;
using System.Diagnostics;

namespace GNet.Utils
{
    public static class Benchmark
    {
        public static TimeSpan Time(Action action, int iterations = 1)
        {
            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < iterations; i++)
            {
                action();
            }

            return sw.Elapsed / iterations;
        }

        public static TimeSpan BatchTime(Network network, Action<Network> trainMethod, int iterations = 1)
        {
            var sw = new Stopwatch();
            int nBatches = 0;

            network.OnStart += OnStart;
            network.OnFinish += OnFinish;
            network.OnBatch += OnBatch;

            for (int i = 0; i < iterations; i++)
            {
                network.Initialize();
                trainMethod(network);
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

            network.OnStart -= OnStart;
            network.OnFinish -= OnFinish;
            network.OnBatch -= OnBatch;

            return sw.Elapsed / nBatches;
        }
    }
}

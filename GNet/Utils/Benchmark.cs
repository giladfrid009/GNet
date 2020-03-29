using System;
using System.Diagnostics;

namespace GNet.Utils
{
    public static class Benchmark
    {
        public static TimeSpan BatchTime(Network network, Action<Network> trainMethod, int count = 1)
        {
            var sw = new Stopwatch();
            var batchTime = new TimeSpan();
            int nBatches = 0;

            network.OnStart += OnStart;
            network.OnFinish += OnFinish;
            network.OnBatch += OnBatch;

            for (int i = 0; i < count; i++)
            {
                network.Initialize();
                trainMethod(network);
            }

            void OnStart(double error)
            {
                sw.Restart();
                nBatches = 0;
            }

            void OnBatch(int batch)
            {
                nBatches++;
            }

            void OnFinish(int epoch, double error)
            {
                sw.Stop();
                batchTime += sw.Elapsed / nBatches;
            }

            network.OnStart -= OnStart;
            network.OnFinish -= OnFinish;
            network.OnBatch -= OnBatch;

            return batchTime;
        }
    }
}

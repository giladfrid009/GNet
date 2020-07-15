using System;
using System.Collections.Generic;
using System.IO;

namespace GNet
{
    public class Logger : IDisposable
    {
        public string DateTimeFormat { get; set; } = "HH:mm:ss.ff";
        public bool LogEpoches { get; set; } = true;
        public bool LogBatches { get; set; } = false;
        public bool Output { get; set; } = true;

        private readonly List<string> logLines;

        private readonly INetwork network;

        public Logger(INetwork network)
        {
            logLines = new List<string>();

            this.network = network;

            network.OnStart += LogStart;
            network.OnFinish += LogFinish;
            network.OnEpoch += LogEpoch;
            network.OnBatch += LogBatch;
        }

        private void AddEntry(string message)
        {
            string line = string.Format($"{DateTime.Now.ToString(DateTimeFormat)} | {message}");

            if (Output)
            {
                Console.WriteLine(line);
            }

            logLines.Add(line);
        }

        private void LogStart(double error)
        {
            AddEntry("Training Started");
            AddEntry($"Initial Error : {error}");
        }

        private void LogFinish(int epoch, double error)
        {
            AddEntry("Training Finished");
            AddEntry($"Elapsed Epoches: {epoch}");
            AddEntry($"Final Error : {error}");
        }

        private void LogEpoch(int epoch, double error)
        {
            if (LogEpoches)
            {
                AddEntry($"Epoch : {epoch} | Error : {error}");
            }
        }

        private void LogBatch(int batch)
        {
            if (LogBatches)
            {
                AddEntry($"Finished Batch {batch}");
            }
        }

        public void Print()
        {
            logLines.ForEach(L => Console.WriteLine(L));
        }

        public void Save(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            logLines.ForEach(L => writer.WriteLine(L));
        }

        public void Dispose()
        {
            network.OnStart -= LogStart;
            network.OnFinish -= LogFinish;
            network.OnEpoch -= LogEpoch;
            network.OnBatch -= LogBatch;
        }
    }
}
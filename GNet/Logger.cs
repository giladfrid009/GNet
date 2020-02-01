using System;
using System.Collections.Generic;
using System.IO;

namespace GNet
{
    public class Logger
    {
        public bool Output { get; set; } = true;
        public bool LogEpoches { get; set; } = false;

        private readonly List<string> logLines = new List<string>();

        public Logger(Network net)
        {
            net.OnStart += LogStart;
            net.OnFinish += LogFinish;
            net.OnEpoch += LogEpoch;
        }

        private void LogEpoch(int epoch, double error)
        {
            if(LogEpoches)
            {
                AddEntry($"Epoch : {epoch} | Error : {error}");
            }
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

        public void AddEntry(string message)
        {
            string line = string.Format($"{DateTime.Now.ToString("HH:mm:ss.ff")} | {message}");

            if (Output)
            {
                Console.WriteLine(line);
            }

            logLines.Add(line);
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
    }
}

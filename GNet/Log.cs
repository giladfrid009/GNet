using System;
using System.Collections.Generic;
using System.IO;

namespace GNet
{
    [Serializable]
    public class Log
    {
        public bool Output { get; set; }

        private readonly List<string> logLines = new List<string>();

        public Log(Network net, bool output = true)
        {
            Output = output;

            net.OnStart += OnStart;
            net.OnFinish += OnFinish;
        }

        private void OnStart(double error)
        {
            AddEntry("Training Started.");
            AddEntry($"Initial Error : {error}");
        }

        private void OnFinish(int epoch, double error)
        {
            AddEntry("Training Finished.");
            AddEntry($"Elapsed Epoches: {epoch}");
            AddEntry($"Final Error : {error}");
        }

        public void AddEntry(string message)
        {
            string line = string.Format($"{DateTime.Now.ToString("HH:mm:ss")} | {message}");

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

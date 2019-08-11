using System;
using System.Collections.Generic;
using System.IO;

namespace GNet
{
    [Serializable]
    public class Log
    {
        private readonly List<string> logLines = new List<string>();

        public void Add(string message, bool print)
        {
            string logLine = DateTime.Now.ToString("HH:mm:ss") + " | " + message;

            if (print)
            {
                Console.WriteLine(logLine);
            }

            logLines.Add(logLine);
        }

        public void Merge(Log log)
        {
            log.logLines.ForEach(L => logLines.Add(L));
        }

        public void Print()
        {
            logLines.ForEach(L => Console.WriteLine(L));
        }

        public void Save(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                logLines.ForEach(L => writer.WriteLine(L));
            }
        }
    }
}

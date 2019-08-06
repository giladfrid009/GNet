using System;
using System.IO;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class Log
    {
        private readonly List<string> logData = new List<string>();

        public void Add(string message, bool timeStamp, bool print)
        {
            string logLine;

            if (timeStamp)
            {
                logLine = DateTime.Now.ToString("HH:mm:ss") + "  " + message;
            }
            else
            {
                logLine = message;
            }

            if (print)
            {
                Console.WriteLine(logLine);
            }

            logData.Add(logLine);
        }

        public void Add(Log log)
        {
            log.logData.ForEach(L => Add(L, false, false));
        }

        public void Print()
        {
            logData.ForEach(L => Console.WriteLine(L));
        }

        public void Save(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                logData.ForEach(L => writer.WriteLine(L));
            }
        }
    }
}

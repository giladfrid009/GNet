using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GNet.Serializer
{
    public static class Binary
    {
        public static void Serialize<TObject>(TObject obj, string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(stream, obj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static TObject Deserialize<TObject>(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    return (TObject)serializer.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message, e);
            }
        }
    }
}

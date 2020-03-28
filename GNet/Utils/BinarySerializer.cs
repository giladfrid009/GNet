using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GNet.Utils
{
    public static class BinarySerializer
    {
        public static MemoryStream Serialize(object obj)
        {
            try
            {
                var stream = new MemoryStream();
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message, e);
            }
        }

        public static TObject Deserialize<TObject>(MemoryStream stream)
        {
            try
            {
                stream.Position = 0;
                var formatter = new BinaryFormatter();
                return (TObject)formatter.Deserialize(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message, e);
            }
        }

        public static void Save(object obj, string filePath)
        {
            try
            {
                using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                var formatter = new BinaryFormatter();
                formatter.Serialize(file, obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message, e);
            }
        }

        public static TObject Load<TObject>(string filePath)
        {
            try
            {
                using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                var formatter = new BinaryFormatter();
                return (TObject)formatter.Deserialize(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message, e);
            }
        }
    }
}
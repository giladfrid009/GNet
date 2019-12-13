using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GNet.Serializers
{
    public static class Binary
    {
        public static MemoryStream Serialize(object obj)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
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
                BinaryFormatter formatter = new BinaryFormatter();
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
                using FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryFormatter formatter = new BinaryFormatter();
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
                using FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryFormatter formatter = new BinaryFormatter();
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

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GNet.Serializers
{
    public static class Binary
    {
        public static Stream Serialize<TObject>(TObject obj)
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

        public static TObject Deserialize<TObject>(Stream stream)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0;
                return (TObject)formatter.Deserialize(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message, e);
            }
        }

        public static void Save<TObject>(TObject obj, string filePath)
        {
            try
            {
                using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
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
                using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryFormatter formatter = new BinaryFormatter();
                return (TObject)formatter.Deserialize(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message, e);
            }
        }
    }
}

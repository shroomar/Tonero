using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Tonero
{
    [Serializable()]
    enum Encoding
    { 
        torrentLink,Ascii
    };
    internal class MPM
    {
        public static string Prefix = "This is an MPM : ";
        public ulong UIN { get; set; }
        public uint TimeLimit { get; set; }
        public bool Personal { get; set; }
        public bool Signed { set; get; }
        public List<string> Tags_original { get; set; }
        public List<string> Tags_response { get; set; }
        public Encoding Coding { get; private set; }
        public byte[] File { get; private set; }
        public MPM(Encoding c, byte[] f)
        {
            Personal = false;
            Signed = false;
            Coding = c;
            Tags_original = new List<string>();
            Tags_response = new List<string>();
            TimeLimit = 0;
            Random r = new Random();
            UIN = (ulong)(ulong.MaxValue * r.NextDouble());
            File = new byte[f.Length];
            for (int i = 0; i < f.Length; i++)
                File[i] = f[i];
        }
        public static MPM Import(string read)
        {
            return SerializationHelper.DeserializeXml<MPM>(read.Substring(Prefix.Length));
        }
        public void SetTimeLimit(uint seconds)
        {
            TimeLimit = seconds;
        }
        public void Add_DeleteTag(string t, bool original)
        {
            if (original)
            {
                if (Tags_original.IndexOf(t) > -1)
                    Tags_original.Add(t);
                else Tags_original.Remove(t);
            }
            else
            {
                if (Tags_response.IndexOf(t) > -1)
                    Tags_response.Add(t);
                else Tags_response.Remove(t);
            }
        }
        public void MakePersonal(string MoneroPublicKey)
        {
            // how to actually do this ? 
            Personal = true;
            // File = Encryption(File, MoneroPublicKey); 
        }
        public void Sign(string MoneroPrivateKey)
        {
            // how to actually do this ? 
            Signed = true;
            // File = Encryption(File, MoneroPrivateKey); 
        }
        public string Export()
        {
            return Prefix + SerializationHelper.SerializeXml(this);
        }
    }


    internal static class SerializationHelper
    {
        public static T DeserializeXml<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static string SerializeXml<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}

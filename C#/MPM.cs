using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Tonero
{
    public enum Enc
    { 
        torrentLink,text
    };
    public class MPM
    {
        public ulong UIN { get; set; }
        public uint TimeLimit { get; set; }
        public bool Personal { get; set; }
        public bool Signed { set; get; }
        public List<string> Tags_original { get; set; }
        public List<string> Tags_response { get; set; }
        public Enc Coding { get; set; }
        public byte[] File { get; set; }
        public MPM(Enc c, byte[] f)
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
        public MPM()
        {
            Tags_original = new List<string>();
            Tags_response = new List<string>();
        }
        public static MPM Import(string read)
        {
            return SerializationHelper.DeserializeXml<MPM>(read);
        }
        public void Add_DeleteTag(string t, bool original)
        {
            if (original)
            {
                if (Tags_original.IndexOf(t) == -1)
                    Tags_original.Add(t);
                else Tags_original.Remove(t);
            }
            else
            {
                if (Tags_response.IndexOf(t) == -1)
                    Tags_response.Add(t);
                else Tags_response.Remove(t);
            }
        }
        public void MakePersonal(string RecieverPublicKey)
        {
            // how to actually do this ? 
            Personal = true;
            // File = Encryption(File, RecieverPublicKey); 
        }
        public void Sign(string SenderPrivateKey)
        {
            // how to actually do this ? 
            Signed = true;
            // File = Encryption(File, SenderPrivateKey); 
        }
        public string Export()
        {
            return SerializationHelper.SerializeXml(this);
        }
    }


    internal static class SerializationHelper
    {
        public static string Prefix = "This is an MPM : ";
        public static T DeserializeXml<T>(this string toDeserialize)
        {
            string xml = FromHexString(toDeserialize);
            xml = xml.Substring(Prefix.Length);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(xml))
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
                string text = Prefix + textWriter.ToString();
                return ToHexString(text);
            }
        }
        private static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        private static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }
    }
}

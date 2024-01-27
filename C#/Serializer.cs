using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tonero
{
    public static class Serializer
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
        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }
        public static (int, string[]) Split(string str, int limit)
        {
            int p = 0; int c = 0;
            string[] parts = new string[str.Length / limit + 1];
            for (int i = 0; i < str.Length; i++)
            {
                parts[p] += str[i];
                c++;
                if (c == limit)
                { p++; c = 0; }
            }
            int rem = parts.Length * limit - str.Length;
            for (int j = 0; j < rem; j++)
                parts[parts.Length - 1] += "0";
            return (rem, parts);
        }
        public static string Join(string[] parts, int rem)
        {
            string all = "";
            for (int i = 0; i < parts.Length; i++)
                all += parts[i];
            return all.Substring(0, all.Length - rem);
        }
    }
}

using System;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace Tonero
{
    public static class Serializer
    {
        static readonly string[] Longr = { "magnet:?xt=", "announce", "tracker"};
        static readonly string[] Short = { "(m", "(a", "(t" };
        
        public static string Prefix = "TTT";
        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.ASCII.GetBytes(str);
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

            return Encoding.ASCII.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }
        public static string TrimTorrent(string x)
        {
            string y = x;
            for (int i = 0; i < Longr.Length; i++)
                y = y.Replace(Longr[i], Short[i]);
            return y;
        }
        public static string BuildTorrent(string x)
        {
            string y = x;
            for (int i = 0; i < Short.Length; i++)
                y = y.Replace(Short[i], Longr[i]);
            return y;
        }
        public static string Compress(string path)
        {
            using (FileStream __fStream = File.Open(path + ".zip", FileMode.Create))
            {
                GZipStream obj = new GZipStream(__fStream, CompressionMode.Compress);

                byte[] bt = File.ReadAllBytes(path);
                obj.Write(bt, 0, bt.Length);

                obj.Close();
                obj.Dispose();
            }
            string myString;
            using (FileStream fs = new FileStream(path + ".zip", FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] bin = br.ReadBytes(Convert.ToInt32(fs.Length));
                myString = Convert.ToBase64String(bin);
            }
            File.Delete(path + ".zip");
            return Prefix + myString;
        }
        public static string Decompress(string path)
        {
            string recover = File.ReadAllText(path);
            byte[] rebin = Convert.FromBase64String(recover.Substring(Prefix.Length));
            using (FileStream fs2 = new FileStream(path + ".zip", FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs2))
                bw.Write(rebin);

            var from = new MemoryStream(rebin);
            var to = new MemoryStream();
            var gZipStream = new GZipStream(from, CompressionMode.Decompress);
            gZipStream.CopyTo(to);
            byte[] bytes = to.ToArray();

            File.WriteAllBytes(path + ".txt", bytes);
            File.Delete(path + ".zip");
            return File.ReadAllText(path + ".txt");
        }
    }
}

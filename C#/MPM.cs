using System;
using System.Collections.Generic;

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
            return Serializer.DeserializeXml<MPM>(read);
        }
        public static MPM Import(string[] parts,int rem)
        {
            string read = Serializer.Join(parts, rem);
            return Serializer.DeserializeXml<MPM>(read);
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
            return Serializer.SerializeXml(this);
        }
        public (int, string[]) Export(int limit)
        {
            string all = Serializer.SerializeXml(this);
            return Serializer.Split(all, limit);
        }
        public static bool IsMPM(string str)
        {
            return str.StartsWith(Serializer.ToHexString(Serializer.Prefix));
        }
    }


    
}

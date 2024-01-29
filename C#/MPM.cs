using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tonero
{
    public class MPM
    {
        public ulong UIN { get; set; }
        public uint TimeLimit { get; set; }
        public bool Personal { get; set; }
        public bool Signed { set; get; }
        public string Tag { get; set; }
        public string Data { get; set; }
        public MPM(string tg, string magnet = null)
        {
            Personal = false;
            Signed = false;
            Tag = tg;
            TimeLimit = 0;
            Random r = new Random();
            UIN = (ulong)(ulong.MaxValue * r.NextDouble());
            Data = magnet;
        }
        public static MPM Import(string str)
        {
            string read = Serializer.FromHexString(str);
           
            string[] x = read.Split('\n');

            string[] y = x[0].Split(',');
            ulong uin = ulong.Parse(y[0]);
            uint time = uint.Parse(y[1]);
            bool pers = y[2] == "1";
            bool sign = y[3] == "1";
            string tag = y[4];
            
            string f = Serializer.BuildTorrent(x[1]);

            MPM m = new MPM(tag, f);
            m.Personal = pers;
            m.Signed = sign;
            m.TimeLimit = time;
            m.UIN = uin;
            return m;
        }
        public static MPM Load(string Path)
        {
            string x = File.ReadAllText(Path);
            return Import(x);
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
            string result = UIN + "," + TimeLimit + ",";
            
            if (Personal) result += "1";
            else result += "0";
            result += ",";

            if (Signed) result += "1";
            else result += "0";
            result += ',';

            result += Tag + '\n';

            result += Serializer.TrimTorrent(Data);
            return Serializer.ToHexString(result);
        }
        public void Save(string Path)
        {
            string e = Export();
            File.WriteAllText(Path, e);
        }
    }


    
}

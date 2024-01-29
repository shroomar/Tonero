using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tonero
{
    public class Test
    {
        public static string CreateMPM()
        {
            string dir = "D://run//";
            string store = dir + "new.txt";
            string path = dir + "torrent.txt";
            
            string file = File.ReadAllText(path);
            MPM m = new MPM("Movie", file);
            m.Save(store);

            string result = Serializer.Compress(store);
            File.Delete(store);
            File.WriteAllText(dir + "save.txt", result);

            return result;
        }
        public static MPM GetMPM()
        {
            string dir = "D://run//";

            string load = Serializer.Decompress(dir + "save.txt");
            MPM back = MPM.Import(load);
            File.Delete(dir + "save.txt.txt");
            File.WriteAllText(dir + "magnet_downlaod.txt", back.Data);

            return back;
        }
    }
}

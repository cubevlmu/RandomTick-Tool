using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.Utils
{
    class LangReader
    {
        private static LangReader reader;
        public static LangReader GetReader()
        {
            if (reader is null)
                reader = new LangReader();
            return reader;
        }

        private Dictionary<string, string> map = new Dictionary<string, string>();
        public void LoadLang(string path)
        {
            if (File.Exists(path))
                ReadLang(path);
        }

        public string GetLnag(string nood)
        {
            if (map.ContainsKey(nood))
                return map[nood];
            return nood;
        }

        private void ReadLang(string file)
        {
            map.Clear();
            var read = File.ReadAllLines(file);
            foreach(var line in read)
            {
                if(!(line is null) && !(line.Length is 0) && line.Contains("=") && !line.StartsWith("#"))
                {
                    map.Add(line.Split('=')[0], line.Split('=')[1]);
                }
            }
            GC.Collect();
        }
    }
}

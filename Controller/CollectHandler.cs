using Name3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.Controller
{
    class CollectHandler
    {
        private static CollectHandler Collect;
        public static CollectHandler GetHandler()
        {
            if (Collect is null)
                Collect = new CollectHandler();
            return Collect;
        }

        private List<string> CollectNames { get; } = new List<string>();
        private List<Collect> Collects { get; } = new List<Collect>();
        public CollectHandler()
        {
            if (!Directory.Exists("Collects"))
                Directory.CreateDirectory("Collects");

            var files = Directory.GetFiles("Collects");
            foreach(string file in files)
            {
                if (file.EndsWith(".collect"))
                {
                    using (CollectReader reader = new CollectReader(file.Replace(".collect", "")))
                    {
                        Collects.Add(reader.ReadCollect());
                    }
                    CollectNames.Add(file);
                    GC.Collect();
                }
            }
            GC.Collect();
        }

        public List<Collect> GetCollects()
        {
            return Collects;
        }

        public bool Delete(int index)
        {
            if (index > Collects.Count)
                return true;
            File.Delete(CollectNames[index]);
            Collects.Remove(Collects[index]);
            CollectNames.Remove(CollectNames[index]);
            return true;
        }

        public bool AddEmptyCollect(string name)
        {
            try
            {
                var collect = new Collect()
                {
                    Auther = "Default",
                    Elements = new List<string>() { "实例" },
                    Name = name
                };

                using (CollectWriter writer = new CollectWriter(collect))
                {
                    if (!writer.WriteTo($"Collects\\{name}"))
                        return false;
                }

                Collects.Add(collect);
                CollectNames.Add($"Collects\\{name}.collect");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RenameCollect(int index, string name)
        {
            try
            {
                if (index > Collects.Count)
                    return false;

                var collect = Collects[index];
                if (File.Exists($"Collects\\{collect.Name}.collect"))
                    File.Delete($"Collects\\{collect.Name}.collect");
                collect.Name = name;

                using (CollectWriter writer = new CollectWriter(collect))
                {
                    if (!writer.WriteTo($"Collects\\{name}"))
                        return false;
                }

                Collects.RemoveAt(index);
                Collects.Add(collect);
                CollectNames.RemoveAt(index);
                CollectNames.Add($"Collects\\{name}.collect");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveAll()
        {
            foreach(var collect in Collects)
            {
                using(CollectWriter writer = new CollectWriter(collect))
                {
                    if (!writer.WriteTo(CollectNames[Collects.IndexOf(collect)].Replace(".collect", "")))
                        throw new Exception("Error When Saving The File");
                }
            }
            GC.Collect();
        }

        public int GetCounts()
        {
            return Collects.Count;
        }

        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (var collect in Collects)
                names.Add(collect.Name);
            return names;
        }
    }
}

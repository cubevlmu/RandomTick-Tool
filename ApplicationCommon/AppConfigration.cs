using Name3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.ApplicationCommon
{
    class AppConfigration
    {
        private static AppConfigration Configration;

        public static AppConfigration Get()
        {
            if (Configration is null)
                Configration = new AppConfigration();
            return Configration;
        }

        private AppConfigration()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
                Console.WriteLine("On Create Data Folder (Not Found)");
                GC.Collect();
            } 
        }

        public void Save()
        {
            using (ConfigCollect Collects = new ConfigCollect())
            {
                Collects.AddObject(new ConfigObject()
                {
                    Nood = "Lang",
                    Type = "String",
                    Value = Lang
                }).AddObject(new ConfigObject()
                {
                    Nood = "Theme",
                    Type = "String",
                    Value = Theme
                }).AddObject(new ConfigObject()
                {
                    Nood = "AutoUpdate",
                    Type = "Bool",
                    Value = AutoUpdate
                }).AddObject(new ConfigObject()
                {
                    Nood = "DarkMode",
                    Type = "Bool",
                    Value = DarkMode
                }).AddObject(new ConfigObject()
                {
                    Nood = "BetterDraw",
                    Type = "Bool",
                    Value = BetterDraw
                }).AddObject(new ConfigObject()
                {
                    Nood = "DeveloperMode",
                    Type = "Bool",
                    Value = DeveloperMode
                }).AddObject(new ConfigObject()
                {
                    Nood = "SyncUrl",
                    Type = "Url",
                    Value = $"(URL:[{SyncUrl.Replace("http://", "")}],TYPE:[HTTP])"
                });

                using (ConfigWriter writer = new ConfigWriter())
                {
                    writer.SaveDocument("Data\\Name3", Collects);
                }
            }
        }

        public void Init()
        {
            if (!File.Exists("Data\\Name3.config"))
            {
                InitDefaultConfig();
                GC.Collect();
            }
            else
            {
                ReadConfig();
                GC.Collect();
            }
        }

        private void ReadConfig()
        {
            ConfigCollect Collects;
            using(ConfigReader reader = new ConfigReader())
            {
                Collects = reader.ReadDocument("Data\\Name3");
            }

            Lang = Collects.GetConfig("Lang").Value.Value as string;
            Theme = Collects.GetConfig("Theme").Value.Value as string;
            
            SyncUrl = Collects.GetConfig("SyncUrl").Value.Value as string;
            SyncUrl = $"http://{SyncUrl.Replace("(URL:[", "").Replace("],TYPE:[HTTP])", "")}";

            AutoUpdate = Boolean.Parse(Collects.GetConfig("AutoUpdate").Value.Value.ToString());
            DarkMode = Boolean.Parse(Collects.GetConfig("DarkMode").Value.Value.ToString());
            BetterDraw = Boolean.Parse(Collects.GetConfig("BetterDraw").Value.Value.ToString());
            DeveloperMode = Boolean.Parse(Collects.GetConfig("DeveloperMode").Value.Value.ToString());

            GC.SuppressFinalize(Collects);
            GC.Collect();
        }

        private void InitDefaultConfig()
        {
            using (ConfigCollect Collects = new ConfigCollect())
            {
                Collects.AddObject(new ConfigObject()
                {
                    Nood = "Lang",
                    Type = "String",
                    Value = "Chinese"
                }).AddObject(new ConfigObject()
                {
                    Nood = "Theme",
                    Type = "String",
                    Value = "Default"
                }).AddObject(new ConfigObject()
                {
                    Nood = "AutoUpdate",
                    Type = "Bool",
                    Value = "True"
                }).AddObject(new ConfigObject()
                {
                    Nood = "DarkMode",
                    Type = "Bool",
                    Value = "False"
                }).AddObject(new ConfigObject()
                {
                    Nood = "BetterDraw",
                    Type = "Bool",
                    Value = "True"
                }).AddObject(new ConfigObject()
                {
                    Nood = "DeveloperMode",
                    Type = "Bool",
                    Value = "False"
                }).AddObject(new ConfigObject()
                {
                    Nood = "SyncUrl",
                    Type = "Url",
                    Value = "(URL:[fbtstudio.gitee.io/webframwork/Name3/Index.json],TYPE:[HTTP])"
                });

                using (ConfigWriter writer = new ConfigWriter())
                {
                    writer.SaveDocument("Data\\Name3", Collects);
                }
            }
            GC.Collect();
        }

        public bool   DeveloperMode { get; set; } = false;
        public bool   BetterDraw    { get; set; } = false;
        public bool   DarkMode      { get; set; } = false;
        public bool   AutoUpdate    { get; set; } = false;
        public string Theme         { get; set; } = "Default";
        public string SyncUrl       { get; set; } = "http://fbtstudio.gitee.io/webframwork/Name3/Index.json";
        public string Lang          { get; set; } = "Chinese";

    }
}

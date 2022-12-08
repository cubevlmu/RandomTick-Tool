using Name3.ApplicationCommon;
using Name3.Utils;
using Name3.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Name3.Controller
{
    class SettingActionHandler
    {
        private Setting Form { get; }
        public SettingActionHandler(Setting setting)
        {
            Form = setting;
        }

        public void ApplySetting()
        {
            SyncLanguages();
            DealWithTheSwitchs();
            DealWithTheDevMode();
        }

        private void DealWithTheDevMode()
        {
            if (Config.DeveloperMode)
            {
                Form.clear.Visible = true;
                Form.clear_hint.Visible = true;

                Form.clear.Click += (s, e) =>
                {
                    MessageBox.Show("The Data Dir Will Be Delete!", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.Copy("Data\\Name3.config", "Name3.config");
                    DirectoryInfo info = new DirectoryInfo("Data");
                    info.Delete(true);
                    Directory.CreateDirectory("Data");
                    File.Copy("Name3.config", "Data\\Name3.config");
                    File.Delete("Name3.config");
                    GC.Collect();
                };
            }
        }

        private void DealWithTheSwitchs()
        {
            Form.AutoUpdate.CheckedChanged += (s, e) =>
            {
                Config.AutoUpdate = Form.AutoUpdate.Checked;
                Config.Save();
            };
            Form.DarkMode.CheckedChanged += (s, e) =>
            {
                Config.DarkMode = Form.DarkMode.Checked;
                Config.Save();
            };
            Form.BetterFontDraw.CheckedChanged += (s, e) =>
            {
                Config.BetterDraw = Form.BetterFontDraw.Checked;
                Config.Save();
            };
            Form.DeveloperMode.CheckedChanged += (s, e) =>
            {
                Config.DeveloperMode = Form.DeveloperMode.Checked;
                Config.Save();
            };

            Form.AutoUpdate.CheckState     = Config.AutoUpdate    ? CheckState.Checked : CheckState.Unchecked;
            Form.DarkMode.CheckState       = Config.DarkMode      ? CheckState.Checked : CheckState.Unchecked;
            Form.BetterFontDraw.CheckState = Config.BetterDraw    ? CheckState.Checked : CheckState.Unchecked;
            Form.DeveloperMode.CheckState  = Config.DeveloperMode ? CheckState.Checked : CheckState.Unchecked;
        }

        public void SyncLanguages()
        {
            string[] files = Directory.GetFiles("Lang");
            List<string> langs = new List<string>();

            foreach(string file in files)
            {
                if (file.EndsWith(".lang"))
                {
                    langs.Add(file.Replace(".lang", "").Replace(@"Lang\", ""));
                }
            }
            GC.Collect();

            Form.metroComboBox1.Items.Clear();
            Form.metroComboBox1.Items.AddRange(langs.ToArray());
            Form.metroComboBox1.SelectedItem = Config.Lang;

            Form.metroComboBox1.SelectedIndexChanged += (s, e) =>
            {
                LangReader.GetReader().LoadLang($"Lang//{Form.metroComboBox1.SelectedItem as string}.lang");

                Config.Lang = Form.metroComboBox1.SelectedItem as string;
                Config.Save();

                ChangeLanguage(Form.metroComboBox1.SelectedItem as string);
            };
        }

        public void ApplyThemesList()
        {
            Form.metroComboBox2.Enabled = false;
        }

        public delegate void Settings(string language);
        public static event Settings ChangeLanguage;
        private AppConfigration Config { get; } = AppConfigration.Get();
    }
}

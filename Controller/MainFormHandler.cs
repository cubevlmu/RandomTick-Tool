using MetroFramework.Controls;
using Name3.ApplicationCommon;
using Name3.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Name3.Controller
{
    class MainFormHandler
    {
        private Form1 Form { get; }
        public KeyValuePair<int, int> Size { get; }

        public MainFormHandler(Form1 form)
        {
            this.Form = form;
            Size = new KeyValuePair<int, int>(form.Width, form.Height);
            if (!Directory.Exists("Lang"))
                Directory.CreateDirectory("Lang");

            SettingActionHandler.ChangeLanguage += (s) =>
            {
                ApplyText(LangReader.GetReader());
            };
            ApplyTheme();
        }

        public void ApplyTheme()
        {

        }

        public void ApplyText(LangReader reader)
        {
            Form.mode0_button.Text = reader.GetLnag("mode0_button.Text");
            Form.mode0_collect.Text = reader.GetLnag("mode0_collect.Text");
            Form.mode0_display.Text = reader.GetLnag("mode0_display.Text");
            Form.mode0_hint0.Text = reader.GetLnag("mode0_hint0.Text");

            Form.mode0_page.Text = reader.GetLnag("mode0_title.Text");
            Form.mode0_title.Text = reader.GetLnag("mode0_title.Text");

            Form.mode1_button.Text = reader.GetLnag("mode1_button.Text");
            Form.mode1_display.Text = reader.GetLnag("mode1_display.Text");
            Form.mode1_hint0.Text = reader.GetLnag("mode1_hint0.Text");
            Form.mode1_hint1.Text = reader.GetLnag("mode1_hint1.Text");
            
            Form.mode1_title.Text = reader.GetLnag("mode1_title.Text");
            Form.mode1_page.Text = reader.GetLnag("mode1_title.Text");
           
            Form.collect_page.Text = reader.GetLnag("collect_title.Text");
            Form.collect_title.Text = reader.GetLnag("collect_title.Text");
            
            Form.editor_editing_add.Text = reader.GetLnag("editor_editing_add.Text");
            Form.editor_editing_delete.Text = reader.GetLnag("editor_editing_delete.Text");
            Form.editor_editing_edit.Text = reader.GetLnag("editor_editing_edit.Text");
            Form.editor_editing_save.Text = reader.GetLnag("editor_editing_save.Text");
            Form.editor_editing_title.Text = reader.GetLnag("editor_editing_title.Text");

            Form.editor_file_add.Text = reader.GetLnag("editor_file_add.Text");
            Form.editor_file_delete.Text = reader.GetLnag("editor_file_delete.Text");
            Form.editor_file_rename.Text = reader.GetLnag("editor_file_rename.Text");

            Form.setting_page.Text = reader.GetLnag("setting_title.Text");
            Form.setting_title.Text = reader.GetLnag("setting_title.Text");

            Form.help_title.Text = reader.GetLnag("help_title.Text");
            Form.help_page.Text = reader.GetLnag("help_title.Text");
            Form.Text = AppConfigration.Get().SyncUrl.Contains("fbtstudio") ? reader.GetLnag("window_title") : $"{reader.GetLnag("window_title")} [;-; You Are Using The 3RD Sync Server]";
        }
    }
}

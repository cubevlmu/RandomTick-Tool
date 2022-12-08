using Name3.Utils;
using Name3.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.Controller
{
    class SettingHandler
    {

        private Setting Form { get; }
        public SettingHandler(Setting setting)
        {
            Form = setting;
            SettingActionHandler.ChangeLanguage += (s) =>
            {
                ApplyLanguage(LangReader.GetReader());
            };
        }

        public void ApplyLanguage(LangReader reader)
        {
            Form.label1.Text = reader.GetLnag("hint_0.Title");
            Form.label2.Text = reader.GetLnag("hint_1.Title");
            Form.label3.Text = reader.GetLnag("hint_2.Title");
            Form.label4.Text = reader.GetLnag("hint_3.Title");
            Form.label5.Text = reader.GetLnag("hint_4.Title");
            Form.label6.Text = reader.GetLnag("hint_5.Title");

            Form.groupBox1.Text = reader.GetLnag("hint_6.Title");
            Form.Text = reader.GetLnag("setting_title");
        }
    }
}

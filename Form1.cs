using MetroFramework.Controls;
using Name3.ApplicationCommon;
using Name3.Controller;
using Name3.Models;
using Name3.Utils;
using Name3.Views;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;

namespace Name3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Editor = new CollectEditor(this);
            Handler = new MainFormHandler(this);
            Reader.LoadLang($"Lang\\{Config.Lang}.lang");
            Handler.ApplyText(Reader);

            HelpDisplay.Navigate(AppInfo.HelpPageUrl);

            GC.Collect();
        }

        public void ReLoadHandler()
        {
            mode0_collect.Items.Clear();
            mode0_collect.Items.AddRange(CollectHandler.GetHandler().GetNames().ToArray());

            mode1_select.Items.Clear();
            mode1_select.Items.AddRange(CollectHandler.GetHandler().GetNames().ToArray());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            naivgator.SelectedIndex = 0;

            mode1_speed.Maximum = 3;
            mode1_speed.Value = 1;
            mode1_speed.Minimum = 1;
            
            ReLoadHandler();

            GC.Collect();
        }

        private AppConfigration Config { get; } = AppConfigration.Get();

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = Config.BetterDraw ? SmoothingMode.AntiAlias : SmoothingMode.HighSpeed;
            e.Graphics.TextRenderingHint = Config.BetterDraw ? TextRenderingHint.AntiAliasGridFit : TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.PixelOffsetMode = Config.BetterDraw ? PixelOffsetMode.HighQuality : PixelOffsetMode.HighSpeed;
            e.Graphics.PageUnit = GraphicsUnit.Display;
            e.Graphics.CompositingQuality = Config.BetterDraw ? CompositingQuality.HighQuality : CompositingQuality.HighSpeed;
            base.OnPaint(e);
        }

        //The Switch Button For Mode1
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (mode0_collect.SelectedIndex is -1)
            {
                SendMessage(Reader.GetLnag("box_msg_0.Text"));
                return;
            }
            CurrentCollect = CollectHandler.GetHandler().GetCollects()[mode0_collect.SelectedIndex];
            var element = CurrentCollect.Elements;
            var index = Random.Next(0, element.Count - 1);
            mode0_display.Text = element[index];
            GC.Collect();
        }

        //The Mode1 Switch
        private void mode1_button_Click(object sender, EventArgs e)
        {
            if (Mode1_Mode is RandomMode.Start) {
                var speed = mode1_speed.Value;
                if (mode1_select.SelectedIndex is -1)
                {
                    SendMessage(Reader.GetLnag("box_msg_0.Text"));
                    return;
                }
                CurrentCollect = CollectHandler.GetHandler().GetCollects()[mode1_select.SelectedIndex];
                IsStop = true;
                var list = CurrentCollect.Elements;
                new Thread(() =>
                {
                    int index = 0;
                    while (IsStop)
                    {
                        mode1_display.Text = list[index];
                        if (mode1_display.Text is null)
                            continue;
                        index++;
                        if (index == list.Count)
                            index = 0;
                        Thread.Sleep(speed * 100);
                    }
                }).Start();
                Mode1_Mode = RandomMode.Stop;
                mode1_button.Text = Reader.GetLnag("mode1_button_Stop.Text");
            } else
            {
                IsStop = false;
                GC.Collect();
                if (mode1_display.Text is null || mode1_display.Text.Length is 0) {

                    var list = CurrentCollect.Elements;
                    mode1_display.Text = list[Random.Next(0, list.Count - 1)];
                }
                Mode1_Mode = RandomMode.Start;
                mode1_button.Text = Reader.GetLnag("mode1_button.Text");
            }
        }

        //The Gift qwq
        private void metroScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var bar = (MetroScrollBar)sender;
            if (e.NewValue == bar.Maximum) //(-^-) Rick Roll [doge]
                Process.Start("https://www.bilibili.com/video/BV1uT4y1P7CX/?spm_id_from=333.337.search-card.all.click");
        }

        public void SendMessage(string content)
        {
            MessageBox.Show(content, Reader.GetLnag("box_title_Info.Text"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void naivgator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.naivgator.SelectedIndex is 2)
                Editor.InitCollectEditor();
            if (this.naivgator.SelectedIndex is 1 || this.naivgator.SelectedIndex is 0)
                ReLoadHandler();
            if (this.naivgator.SelectedIndex is 4)
            {
                this.naivgator.SelectedIndex = 0;
                using(Setting setting = new Setting(this))
                {
                    setting.ShowDialog();
                }
            }
        }

        private void editor_files_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Editor.FileListSelect();
        }

        private void editor_file_delete_Click(object sender, EventArgs e)
        {
            Editor.DeleteCollect();
        }

        private void editor_file_add_Click(object sender, EventArgs e)
        {
            Editor.AddCollect();
        }

        private void editor_file_rename_Click(object sender, EventArgs e)
        {
            Editor.RenameCollect();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                if(IsFirstTime)
                {
                    IsFirstTime = false;
                    return;
                }
                this.Width = Handler.Size.Key;
                this.Height = Handler.Size.Value;
            }
            catch (NullReferenceException) { return; }
        }

        private void editor_editing_add_Click(object sender, EventArgs e)
        {
            Editor.AddLines(editor_editing_input.Text);
        }

        private void editor_editing_delete_Click(object sender, EventArgs e)
        {
            Editor.RemoveLines();
        }
        private void editor_editing_save_Click(object sender, EventArgs e)
        {
            Editor.Save();
        }

        private bool IsFirstTime { set; get; } = true;
        private CollectEditor Editor { get; }
        private bool IsStop { get; set; } = false;
        private RandomMode Mode1_Mode { get; set; } = RandomMode.Start;
        private MainFormHandler Handler { get; }
        private LangReader Reader { get; } = LangReader.GetReader();
        private Random Random { get; } = new Random();
        private Collect CurrentCollect { get; set; }

    }
}

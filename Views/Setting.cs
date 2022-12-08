using MetroFramework.Forms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Name3.Controller;
using Name3.Utils;
using Name3.ApplicationCommon;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Name3.Views
{
    public partial class Setting : Form
    {
        private SettingHandler Handler { get; }
        private SettingActionHandler ActionHandler { get; }
        private Form Form { get; }
        public Setting(Form1 form)
        {
            this.Form = form;
            InitializeComponent();
            ActionHandler = new SettingActionHandler(this);
            Handler = new SettingHandler(this);
            Handler.ApplyLanguage(LangReader.GetReader());
            ActionHandler.ApplySetting();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Graphics = Form.CreateGraphics();
            MessageBox.Show(
                $"Version 0.1 \n " +
                $"Developing Code 5 \n " +
                $"DPI x: {Graphics.DpiX} y: {Graphics.DpiY} \n " +
                $"GDI Smooth Mode: {Graphics.SmoothingMode} \n " +
                $"GDI TextContrast: {Graphics.TextContrast} \n " +
                $"GDI TextRenderingHint: {Graphics.TextRenderingHint} \n " +
                $"\n" +
                $"By CubeVlmu & FlybirdStudio 2015 ~ 2022",
                "ABOUT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
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

        private void Setting_Paint(object sender, PaintEventArgs e)
        {
            if (Graphics != e.Graphics || Graphics is null)
                Graphics = e.Graphics;
        }

        private Graphics Graphics { set; get; }
    }
}

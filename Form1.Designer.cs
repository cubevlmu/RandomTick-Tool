
using Name3.Utils;
using Name3.Views;

namespace Name3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.naivgator = new MetroFramework.Controls.MetroTabControl();
            this.mode0_page = new MetroFramework.Controls.MetroTabPage();
            this.mode0_display = new MetroFramework.Controls.MetroLink();
            this.mode0_button = new MetroFramework.Controls.MetroButton();
            this.mode0_collect = new MetroFramework.Controls.MetroComboBox();
            this.metroScrollBar1 = new MetroFramework.Controls.MetroScrollBar();
            this.mode0_hint0 = new MetroFramework.Controls.MetroLabel();
            this.mode0_title = new MetroFramework.Controls.MetroLabel();
            this.mode1_page = new MetroFramework.Controls.MetroTabPage();
            this.mode1_select = new MetroFramework.Controls.MetroComboBox();
            this.mode1_hint1 = new MetroFramework.Controls.MetroLabel();
            this.mode1_hint0 = new MetroFramework.Controls.MetroLabel();
            this.mode1_button = new MetroFramework.Controls.MetroButton();
            this.mode1_display = new MetroFramework.Controls.MetroLink();
            this.mode1_title = new MetroFramework.Controls.MetroLabel();
            this.mode1_speed = new MetroFramework.Controls.MetroTrackBar();
            this.collect_page = new MetroFramework.Controls.MetroTabPage();
            this.editor_file_delete = new MetroFramework.Controls.MetroButton();
            this.editor_file_add = new MetroFramework.Controls.MetroButton();
            this.editor_file_rename = new MetroFramework.Controls.MetroButton();
            this.editor_editing_add = new MetroFramework.Controls.MetroButton();
            this.editor_editing_save = new MetroFramework.Controls.MetroButton();
            this.editor_editing_input = new MetroFramework.Controls.MetroTextBox();
            this.editor_editing_delete = new MetroFramework.Controls.MetroButton();
            this.editor_editing_edit = new MetroFramework.Controls.MetroButton();
            this.editor_editing_title = new MetroFramework.Controls.MetroLabel();
            this.editor_files = new System.Windows.Forms.ListBox();
            this.editor_elements = new System.Windows.Forms.CheckedListBox();
            this.collect_title = new MetroFramework.Controls.MetroLabel();
            this.help_page = new MetroFramework.Controls.MetroTabPage();
            this.HelpDisplay = new System.Windows.Forms.WebBrowser();
            this.help_title = new MetroFramework.Controls.MetroLabel();
            this.setting_page = new MetroFramework.Controls.MetroTabPage();
            this.setting_title = new MetroFramework.Controls.MetroLabel();
            this.naivgator.SuspendLayout();
            this.mode0_page.SuspendLayout();
            this.mode1_page.SuspendLayout();
            this.collect_page.SuspendLayout();
            this.help_page.SuspendLayout();
            this.setting_page.SuspendLayout();
            this.SuspendLayout();
            // 
            // naivgator
            // 
            this.naivgator.AllowDrop = true;
            this.naivgator.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.naivgator.Controls.Add(this.mode0_page);
            this.naivgator.Controls.Add(this.mode1_page);
            this.naivgator.Controls.Add(this.collect_page);
            this.naivgator.Controls.Add(this.help_page);
            this.naivgator.Controls.Add(this.setting_page);
            this.naivgator.CustomBackground = true;
            this.naivgator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.naivgator.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.naivgator.FontWeight = MetroFramework.MetroTabControlWeight.Regular;
            this.naivgator.HotTrack = true;
            this.naivgator.Location = new System.Drawing.Point(0, 0);
            this.naivgator.Name = "naivgator";
            this.naivgator.Padding = new System.Drawing.Point(6, 8);
            this.naivgator.SelectedIndex = 0;
            this.naivgator.Size = new System.Drawing.Size(1020, 579);
            this.naivgator.TabIndex = 0;
            this.naivgator.SelectedIndexChanged += new System.EventHandler(this.naivgator_SelectedIndexChanged);
            // 
            // mode0_page
            // 
            this.mode0_page.Controls.Add(this.mode0_display);
            this.mode0_page.Controls.Add(this.mode0_button);
            this.mode0_page.Controls.Add(this.mode0_collect);
            this.mode0_page.Controls.Add(this.metroScrollBar1);
            this.mode0_page.Controls.Add(this.mode0_hint0);
            this.mode0_page.Controls.Add(this.mode0_title);
            this.mode0_page.Enabled = true;
            this.mode0_page.HorizontalScrollbarBarColor = true;
            this.mode0_page.Location = new System.Drawing.Point(4, 43);
            this.mode0_page.Name = "mode0_page";
            this.mode0_page.Size = new System.Drawing.Size(1012, 532);
            this.mode0_page.TabIndex = 0;
            this.mode0_page.Text = "mode0_page.Text";
            this.mode0_page.VerticalScrollbarBarColor = true;
            this.mode0_page.Visible = true;
            // 
            // mode0_display
            // 
            this.mode0_display.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.mode0_display.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.mode0_display.Location = new System.Drawing.Point(265, 97);
            this.mode0_display.Name = "mode0_display";
            this.mode0_display.Size = new System.Drawing.Size(468, 159);
            this.mode0_display.TabIndex = 7;
            this.mode0_display.Text = "mode0_display.Text";
            // 
            // mode0_button
            // 
            this.mode0_button.AutoSize = true;
            this.mode0_button.Highlight = true;
            this.mode0_button.Location = new System.Drawing.Point(265, 282);
            this.mode0_button.Name = "mode0_button";
            this.mode0_button.Size = new System.Drawing.Size(470, 68);
            this.mode0_button.TabIndex = 6;
            this.mode0_button.Text = "mode0_button.Text";
            this.mode0_button.UseWaitCursor = true;
            this.mode0_button.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // mode0_collect
            // 
            this.mode0_collect.FormattingEnabled = true;
            this.mode0_collect.ItemHeight = 23;
            this.mode0_collect.Location = new System.Drawing.Point(390, 366);
            this.mode0_collect.Name = "mode0_collect";
            this.mode0_collect.Size = new System.Drawing.Size(340, 29);
            this.mode0_collect.TabIndex = 5;
            // 
            // metroScrollBar1
            // 
            this.metroScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.metroScrollBar1.Location = new System.Drawing.Point(1002, 0);
            this.metroScrollBar1.Name = "metroScrollBar1";
            this.metroScrollBar1.Orientation = MetroFramework.Controls.MetroScrollOrientation.Vertical;
            this.metroScrollBar1.Size = new System.Drawing.Size(10, 532);
            this.metroScrollBar1.TabIndex = 4;
            this.metroScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.metroScrollBar1_Scroll);
            // 
            // mode0_hint0
            // 
            this.mode0_hint0.AutoSize = true;
            this.mode0_hint0.Location = new System.Drawing.Point(255, 373);
            this.mode0_hint0.Name = "mode0_hint0";
            this.mode0_hint0.Size = new System.Drawing.Size(110, 19);
            this.mode0_hint0.TabIndex = 3;
            this.mode0_hint0.Text = "mode0_hint0.Text";
            // 
            // mode0_title
            // 
            this.mode0_title.AutoSize = true;
            this.mode0_title.Location = new System.Drawing.Point(8, 12);
            this.mode0_title.Name = "mode0_title";
            this.mode0_title.Size = new System.Drawing.Size(103, 19);
            this.mode0_title.TabIndex = 2;
            this.mode0_title.Text = "mode0_title.Text";
            // 
            // mode1_page
            // 
            this.mode1_page.Controls.Add(this.mode1_select);
            this.mode1_page.Controls.Add(this.mode1_hint1);
            this.mode1_page.Controls.Add(this.mode1_hint0);
            this.mode1_page.Controls.Add(this.mode1_button);
            this.mode1_page.Controls.Add(this.mode1_display);
            this.mode1_page.Controls.Add(this.mode1_title);
            this.mode1_page.Controls.Add(this.mode1_speed);
            this.mode1_page.Enabled = true;
            this.mode1_page.HorizontalScrollbarBarColor = true;
            this.mode1_page.Location = new System.Drawing.Point(4, 43);
            this.mode1_page.Name = "mode1_page";
            this.mode1_page.Size = new System.Drawing.Size(1012, 532);
            this.mode1_page.TabIndex = 1;
            this.mode1_page.Text = "循环模式";
            this.mode1_page.VerticalScrollbarBarColor = true;
            this.mode1_page.Visible = false;
            // 
            // mode1_select
            // 
            this.mode1_select.FormattingEnabled = true;
            this.mode1_select.ItemHeight = 23;
            this.mode1_select.Location = new System.Drawing.Point(406, 411);
            this.mode1_select.Name = "mode1_select";
            this.mode1_select.Size = new System.Drawing.Size(324, 29);
            this.mode1_select.TabIndex = 12;
            // 
            // mode1_hint1
            // 
            this.mode1_hint1.AutoSize = true;
            this.mode1_hint1.Location = new System.Drawing.Point(264, 418);
            this.mode1_hint1.Name = "mode1_hint1";
            this.mode1_hint1.Size = new System.Drawing.Size(65, 19);
            this.mode1_hint1.TabIndex = 11;
            this.mode1_hint1.Text = "选择签库";
            // 
            // mode1_hint0
            // 
            this.mode1_hint0.AutoSize = true;
            this.mode1_hint0.Location = new System.Drawing.Point(264, 366);
            this.mode1_hint0.Name = "mode1_hint0";
            this.mode1_hint0.Size = new System.Drawing.Size(37, 19);
            this.mode1_hint0.TabIndex = 10;
            this.mode1_hint0.Text = "速度";
            // 
            // mode1_button
            // 
            this.mode1_button.Highlight = true;
            this.mode1_button.Location = new System.Drawing.Point(264, 279);
            this.mode1_button.Name = "mode1_button";
            this.mode1_button.Size = new System.Drawing.Size(468, 63);
            this.mode1_button.TabIndex = 9;
            this.mode1_button.Text = "开始(:>";
            this.mode1_button.Click += new System.EventHandler(this.mode1_button_Click);
            // 
            // mode1_display
            // 
            this.mode1_display.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.mode1_display.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.mode1_display.Location = new System.Drawing.Point(264, 109);
            this.mode1_display.Name = "mode1_display";
            this.mode1_display.Size = new System.Drawing.Size(468, 159);
            this.mode1_display.TabIndex = 8;
            this.mode1_display.Text = "^-^点击按钮开始吧~";
            // 
            // mode1_title
            // 
            this.mode1_title.AutoSize = true;
            this.mode1_title.Location = new System.Drawing.Point(8, 12);
            this.mode1_title.Name = "mode1_title";
            this.mode1_title.Size = new System.Drawing.Size(65, 19);
            this.mode1_title.TabIndex = 7;
            this.mode1_title.Text = "循环模式";
            // 
            // mode1_speed
            // 
            this.mode1_speed.BackColor = System.Drawing.Color.Transparent;
            this.mode1_speed.Location = new System.Drawing.Point(374, 364);
            this.mode1_speed.Name = "mode1_speed";
            this.mode1_speed.Size = new System.Drawing.Size(358, 35);
            this.mode1_speed.TabIndex = 6;
            this.mode1_speed.Text = "metroTrackBar1";
            // 
            // collect_page
            // 
            this.collect_page.Controls.Add(this.editor_file_delete);
            this.collect_page.Controls.Add(this.editor_file_add);
            this.collect_page.Controls.Add(this.editor_file_rename);
            this.collect_page.Controls.Add(this.editor_editing_add);
            this.collect_page.Controls.Add(this.editor_editing_save);
            this.collect_page.Controls.Add(this.editor_editing_input);
            this.collect_page.Controls.Add(this.editor_editing_delete);
            this.collect_page.Controls.Add(this.editor_editing_edit);
            this.collect_page.Controls.Add(this.editor_editing_title);
            this.collect_page.Controls.Add(this.editor_files);
            this.collect_page.Controls.Add(this.editor_elements);
            this.collect_page.Controls.Add(this.collect_title);
            this.collect_page.Enabled = true;
            this.collect_page.HorizontalScrollbarBarColor = true;
            this.collect_page.Location = new System.Drawing.Point(4, 43);
            this.collect_page.Name = "collect_page";
            this.collect_page.Size = new System.Drawing.Size(1012, 532);
            this.collect_page.TabIndex = 4;
            this.collect_page.Text = "签池";
            this.collect_page.VerticalScrollbarBarColor = true;
            this.collect_page.Visible = false;
            // 
            // editor_file_delete
            // 
            this.editor_file_delete.Location = new System.Drawing.Point(27, 432);
            this.editor_file_delete.Name = "editor_file_delete";
            this.editor_file_delete.Size = new System.Drawing.Size(75, 52);
            this.editor_file_delete.TabIndex = 13;
            this.editor_file_delete.Text = "删除";
            this.editor_file_delete.Click += new System.EventHandler(this.editor_file_delete_Click);
            // 
            // editor_file_add
            // 
            this.editor_file_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editor_file_add.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.editor_file_add.Location = new System.Drawing.Point(109, 432);
            this.editor_file_add.Name = "editor_file_add";
            this.editor_file_add.Size = new System.Drawing.Size(75, 52);
            this.editor_file_add.TabIndex = 14;
            this.editor_file_add.Text = "添加";
            this.editor_file_add.Click += new System.EventHandler(this.editor_file_add_Click);
            // 
            // editor_file_rename
            // 
            this.editor_file_rename.Location = new System.Drawing.Point(191, 432);
            this.editor_file_rename.Name = "editor_file_rename";
            this.editor_file_rename.Size = new System.Drawing.Size(79, 52);
            this.editor_file_rename.TabIndex = 15;
            this.editor_file_rename.Text = "改名";
            this.editor_file_rename.Click += new System.EventHandler(this.editor_file_rename_Click);
            // 
            // editor_editing_add
            // 
            this.editor_editing_add.Location = new System.Drawing.Point(292, 431);
            this.editor_editing_add.Name = "editor_editing_add";
            this.editor_editing_add.Size = new System.Drawing.Size(130, 53);
            this.editor_editing_add.TabIndex = 17;
            this.editor_editing_add.Text = "添加";
            this.editor_editing_add.Click += new System.EventHandler(this.editor_editing_add_Click);
            // 
            // editor_editing_save
            // 
            this.editor_editing_save.Location = new System.Drawing.Point(739, 432);
            this.editor_editing_save.Name = "editor_editing_save";
            this.editor_editing_save.Size = new System.Drawing.Size(244, 53);
            this.editor_editing_save.TabIndex = 20;
            this.editor_editing_save.Text = "保存";
            this.editor_editing_save.Click += new System.EventHandler(this.editor_editing_save_Click);
            // 
            // editor_editing_input
            // 
            this.editor_editing_input.Location = new System.Drawing.Point(292, 337);
            this.editor_editing_input.Multiline = true;
            this.editor_editing_input.Name = "editor_editing_input";
            this.editor_editing_input.Size = new System.Drawing.Size(691, 81);
            this.editor_editing_input.TabIndex = 16;
            // 
            // editor_editing_delete
            // 
            this.editor_editing_delete.Location = new System.Drawing.Point(428, 432);
            this.editor_editing_delete.Name = "editor_editing_delete";
            this.editor_editing_delete.Size = new System.Drawing.Size(169, 53);
            this.editor_editing_delete.TabIndex = 18;
            this.editor_editing_delete.Text = "删除选中项";
            this.editor_editing_delete.Click += new System.EventHandler(this.editor_editing_delete_Click);
            // 
            // editor_editing_edit
            // 
            this.editor_editing_edit.Enabled = false;
            this.editor_editing_edit.Location = new System.Drawing.Point(603, 432);
            this.editor_editing_edit.Name = "editor_editing_edit";
            this.editor_editing_edit.Size = new System.Drawing.Size(130, 53);
            this.editor_editing_edit.TabIndex = 19;
            this.editor_editing_edit.Text = "修改";
            // 
            // editor_editing_title
            // 
            this.editor_editing_title.AutoSize = true;
            this.editor_editing_title.Location = new System.Drawing.Point(290, 49);
            this.editor_editing_title.Name = "editor_editing_title";
            this.editor_editing_title.Size = new System.Drawing.Size(113, 19);
            this.editor_editing_title.TabIndex = 12;
            this.editor_editing_title.Text = "正在编辑: [name]";
            // 
            // editor_files
            // 
            this.editor_files.FormattingEnabled = true;
            this.editor_files.ItemHeight = 24;
            this.editor_files.Location = new System.Drawing.Point(27, 54);
            this.editor_files.Name = "editor_files";
            this.editor_files.Size = new System.Drawing.Size(243, 364);
            this.editor_files.TabIndex = 11;
            this.editor_files.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.editor_files_MouseDoubleClick);
            // 
            // editor_elements
            // 
            this.editor_elements.FormattingEnabled = true;
            this.editor_elements.Location = new System.Drawing.Point(292, 93);
            this.editor_elements.Name = "editor_elements";
            this.editor_elements.Size = new System.Drawing.Size(691, 228);
            this.editor_elements.TabIndex = 10;
            // 
            // collect_title
            // 
            this.collect_title.AutoSize = true;
            this.collect_title.Location = new System.Drawing.Point(8, 12);
            this.collect_title.Name = "collect_title";
            this.collect_title.Size = new System.Drawing.Size(37, 19);
            this.collect_title.TabIndex = 9;
            this.collect_title.Text = "签池";
            // 
            // help_page
            // 
            this.help_page.Controls.Add(this.HelpDisplay);
            this.help_page.Controls.Add(this.help_title);
            this.help_page.Enabled = true;
            this.help_page.HorizontalScrollbarBarColor = true;
            this.help_page.Location = new System.Drawing.Point(4, 43);
            this.help_page.Name = "help_page";
            this.help_page.Size = new System.Drawing.Size(1012, 532);
            this.help_page.TabIndex = 3;
            this.help_page.Text = "帮助";
            this.help_page.VerticalScrollbarBarColor = true;
            this.help_page.Visible = false;
            // 
            // HelpDisplay
            // 
            this.HelpDisplay.Location = new System.Drawing.Point(42, 66);
            this.HelpDisplay.MinimumSize = new System.Drawing.Size(20, 20);
            this.HelpDisplay.Name = "HelpDisplay";
            this.HelpDisplay.ScriptErrorsSuppressed = true;
            this.HelpDisplay.Size = new System.Drawing.Size(962, 458);
            this.HelpDisplay.TabIndex = 10;
            this.HelpDisplay.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // help_title
            // 
            this.help_title.AutoSize = true;
            this.help_title.Location = new System.Drawing.Point(8, 12);
            this.help_title.Name = "help_title";
            this.help_title.Size = new System.Drawing.Size(37, 19);
            this.help_title.TabIndex = 9;
            this.help_title.Text = "帮助";
            // 
            // setting_page
            // 
            this.setting_page.Controls.Add(this.setting_title);
            this.setting_page.Enabled = true;
            this.setting_page.HorizontalScrollbarBarColor = true;
            this.setting_page.Location = new System.Drawing.Point(4, 43);
            this.setting_page.Name = "setting_page";
            this.setting_page.Size = new System.Drawing.Size(1012, 532);
            this.setting_page.TabIndex = 2;
            this.setting_page.Text = "设置";
            this.setting_page.VerticalScrollbarBarColor = true;
            this.setting_page.Visible = false;
            // 
            // setting_title
            // 
            this.setting_title.AutoSize = true;
            this.setting_title.Location = new System.Drawing.Point(8, 12);
            this.setting_title.Name = "setting_title";
            this.setting_title.Size = new System.Drawing.Size(37, 19);
            this.setting_title.TabIndex = 8;
            this.setting_title.Text = "设置";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1020, 579);
            this.Controls.Add(this.naivgator);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Name";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.naivgator.ResumeLayout(false);
            this.mode0_page.ResumeLayout(false);
            this.mode0_page.PerformLayout();
            this.mode1_page.ResumeLayout(false);
            this.mode1_page.PerformLayout();
            this.collect_page.ResumeLayout(false);
            this.collect_page.PerformLayout();
            this.help_page.ResumeLayout(false);
            this.help_page.PerformLayout();
            this.setting_page.ResumeLayout(false);
            this.setting_page.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public MetroFramework.Controls.MetroTabControl naivgator;
        public MetroFramework.Controls.MetroTabPage mode0_page;
        public MetroFramework.Controls.MetroTabPage mode1_page;
        public MetroFramework.Controls.MetroTabPage setting_page;
        public MetroFramework.Controls.MetroTabPage help_page;
        public MetroFramework.Controls.MetroLabel mode0_hint0;
        public MetroFramework.Controls.MetroLabel mode0_title;
        public MetroFramework.Controls.MetroScrollBar metroScrollBar1;
        public MetroFramework.Controls.MetroTrackBar mode1_speed;
        public MetroFramework.Controls.MetroComboBox mode0_collect;
        public MetroFramework.Controls.MetroButton mode0_button;
        public MetroFramework.Controls.MetroLink mode0_display;
        public MetroFramework.Controls.MetroLabel mode1_title;
        public MetroFramework.Controls.MetroLabel mode1_hint0;
        public MetroFramework.Controls.MetroButton mode1_button;
        public MetroFramework.Controls.MetroLink mode1_display;
        public MetroFramework.Controls.MetroComboBox mode1_select;
        public MetroFramework.Controls.MetroLabel mode1_hint1;
        public MetroFramework.Controls.MetroLabel setting_title;
        public System.Windows.Forms.WebBrowser HelpDisplay;
        public MetroFramework.Controls.MetroLabel help_title;
        public MetroFramework.Controls.MetroTabPage collect_page;
        public MetroFramework.Controls.MetroLabel collect_title;
        public MetroFramework.Controls.MetroLabel editor_editing_title;
        public System.Windows.Forms.ListBox editor_files;
        public System.Windows.Forms.CheckedListBox editor_elements;
        public MetroFramework.Controls.MetroButton editor_file_rename;
        public MetroFramework.Controls.MetroButton editor_file_delete;
        public MetroFramework.Controls.MetroTextBox editor_editing_input;
        public MetroFramework.Controls.MetroButton editor_editing_edit;
        public MetroFramework.Controls.MetroButton editor_editing_delete;
        public MetroFramework.Controls.MetroButton editor_editing_add;
        public MetroFramework.Controls.MetroButton editor_editing_save;
        public MetroFramework.Controls.MetroButton editor_file_add;
    }
}


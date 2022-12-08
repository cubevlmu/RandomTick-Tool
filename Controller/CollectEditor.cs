using Microsoft.VisualBasic;
using Name3.Models;
using Name3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Name3.Controller
{
    partial class CollectEditor
    {

        private Form1 Form { get; }
        public CollectEditor(Form1 form)
        {
            this.Form = form;
            Form.editor_files.SelectedIndexChanged += (s, e) 
                => CurrentCollect = CollectHandler.GetHandler().GetCollects()[Form.editor_files.SelectedIndex];
        }

        private void UnLoadCollectEditor()
        {

        }

        public void InitCollectEditor()
        {
            var handler = CollectHandler.GetHandler();
            Form.editor_files.Items.Clear();
            Form.editor_files.Items.AddRange(handler.GetNames().ToArray());
        }

        public void FileListSelect()
        {
            if (Form.editor_files.SelectedIndex is -1)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_1.Text"));
                return;
            }

            var handler = CollectHandler.GetHandler();
            var collect = handler.GetCollects()[Form.editor_files.SelectedIndex];

            var temp = Reader.GetLnag("editor_editing_title_template.Text");
            Form.editor_editing_title.Text = $"{temp} {collect.Name}";

            Form.editor_elements.Items.Clear();
            Form.editor_elements.Items.AddRange(collect.Elements.ToArray());
        }

        public void DeleteCollect()
        {
            if (Form.editor_files.SelectedIndex is -1)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_1.Text"));
                return;
            }

            var handler = CollectHandler.GetHandler();
            var result = handler.Delete(Form.editor_files.SelectedIndex);

            if (result)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_3.Text"));
                InitCollectEditor();
            }
            else
            {
                Form.SendMessage(Reader.GetLnag("box_msg_2.Text"));
            }
        }

        public void AddCollect()
        {
            var name = GetInputBox(Reader.GetLnag("inputbox_msg_0.Text"));

            if(name.Length is 0 || name is null)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_4.Text"));
                return;
            }

            var handler = CollectHandler.GetHandler();
            var result = handler.AddEmptyCollect(name);

            if (result)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_5.Text"));
                InitCollectEditor();
            }
            else
            {
                Form.SendMessage(Reader.GetLnag("box_msg_6.Text"));
            }
        }

        public void RenameCollect()
        {
            if (Form.editor_files.SelectedIndex is -1)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_1.Text"));
                return;
            }

            var name = GetInputBox(Reader.GetLnag("inputbox_msg_1.Text"));

            if (name.Length is 0 || name is null)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_4.Text"));
                return;
            }

            var handler = CollectHandler.GetHandler();
            var result = handler.RenameCollect(Form.editor_files.SelectedIndex, name);

            if (result)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_8.Text"));
                InitCollectEditor();
            }
            else
            {
                Form.SendMessage(Reader.GetLnag("box_msg_7.Text"));
            }
        }

        public string GetInputBox(string message)
        {
            return Interaction.InputBox(message, Reader.GetLnag("inputbox_title.Text"));
        }

        private Collect CurrentCollect { get; set; }
        private LangReader Reader { get; } = LangReader.GetReader();
    }
}

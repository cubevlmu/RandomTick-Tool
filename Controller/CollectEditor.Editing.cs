using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.Controller
{
    partial class CollectEditor
    {
        [Obsolete]
        public void AddLine(string line)
        {
            CurrentCollect.Elements.Add(line);
            Form.SendMessage(Reader.GetLnag("box_msg_5.Text"));
            FileListSelect();
        }

        public void AddLines(string lines)
        {
            if(lines is null)
            {
                Form.SendMessage(Reader.GetLnag("box_msg_4.Text"));
                return;
            }

            var toAdd = lines.Split('\n');
            foreach (var line in toAdd)
                CurrentCollect.Elements.Add(line);
            Form.SendMessage(Reader.GetLnag("box_msg_5.Text"));
            FileListSelect();
        }

        public void RemoveLines()
        {
            var items = Form.editor_elements.SelectedItems;
            foreach(var item in items)
                CurrentCollect.Elements.Remove(item.ToString());
            Form.SendMessage(Reader.GetLnag("box_msg_5.Text"));
            FileListSelect();
        }

        public void Save()
        {
            CollectHandler.GetHandler().SaveAll();
            GC.Collect();
            Form.SendMessage(Reader.GetLnag("box_msg_9.Text"));
            InitCollectEditor();
            Form.editor_files.SelectedIndex = 0;
            FileListSelect();
        }
    }
}

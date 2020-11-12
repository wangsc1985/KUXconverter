using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace KUXconverter
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private delegate void FormControlInvoker();
        //开始转换
        private void Button1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                //列表为空不运行
                if (listBoxFiles.Items.Count > 0)
                {
                    var size = listBoxFiles.Items.Count;
                    for (int i = 0; i < size; i++)
                    {
                        Cmd ccc = new Cmd();
                        var path = listBoxFiles.Items[i].ToString();
                        this.Invoke((EventHandler)delegate {
                            textStatus.Text = $"正在转换{Path.GetFileName(path)}…";
                        });
                        var cmd = $" -y -i \"{path}\" -c:v copy -c:a copy -threads 2 \"{path.Replace(".kux", "")}.mp4\"";
                        ccc.RunCmd(cmd);
                    }
                    textStatus.Text = "";
                    MessageBox.Show("转换完毕！");
                }
            })).Start();

        }

        private void listBox_DragDrop(object sender, DragEventArgs e)
        {
            var dragFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in dragFiles)
            {
                if(!listBoxFiles.Items.Contains(file))
                listBoxFiles.Items.Add(file);
            }
        }

        private void listBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBoxFiles.Items.Clear();
        }
    }
}

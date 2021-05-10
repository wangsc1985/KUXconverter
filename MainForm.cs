using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        private void listBox_DragDrop(object sender, DragEventArgs e)
        {
            var dragFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in dragFiles)
            {
                if (!listBoxFiles.Items.Contains(file))
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

        private void btnMerge_Click(object sender, EventArgs e)
        {
            //ffmpeg - f concat - i filelist.txt - c copy output.mkv

            //              filelist.text内容
            //              file  'D:\delphisr\腾讯下载地址解析子串\k0028qzpkdl.321002.1.ts'
            //              file  'D:\delphisr\腾讯下载地址解析子串\k0028qzpkdl.321002.2.ts'
            //              file  'D:\delphisr\腾讯下载地址解析子串\k0028qzpkdl.321002.3.ts'

            new Thread(new ThreadStart(() =>
            {
                Cmd ccc = new Cmd();
                var file = "filelist.txt";
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                if (listBoxFiles.Items.Count > 0)
                {
                    //
                    var path = listBoxFiles.Items[0].ToString();
                    var ext = Path.GetExtension(listBoxFiles.Items[0].ToString());
                    var list = new List<string>();
                    for (int i = 0; i < listBoxFiles.Items.Count; i++)
                    {
                        list.Add($"file '{listBoxFiles.Items[i]}'");
                    }
                    File.AppendAllLines(file, list,Encoding.Default);

                    //
                    //ffmpeg -f concat -safe 0 -i filelist.txt -c copy aaa.mp4
                    var cmd = $" -y -f concat -safe 0 -i {file} -c copy  -threads 2 \"{path.Replace(ext, "")}合并.mp4\"";
                    this.Invoke((EventHandler)delegate
                    {
                        textStatus.Text = "正在合并...";
                    });
                    var result = ccc.RunCmd(cmd);

                    //
                    textStatus.Text = "合并完毕！";
                    MessageBox.Show(result);
                    //File.Delete(file);
                }
            })).Start();
        }

        private void btnOutAudio_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                if (listBoxFiles.Items.Count > 0)
                {
                    var ext = Path.GetExtension(listBoxFiles.Items[0].ToString());
                    for (int i = 0; i < listBoxFiles.Items.Count; i++)
                    {
                        var source = listBoxFiles.Items[i].ToString();
                        var target = $"{ source.Replace(ext, "") }.m4a";
                        var target1 = $"{ source.Replace(ext, "") }.mp3";
                        this.Invoke((EventHandler)delegate
                        {
                            textStatus.Text = $"正在提取{Path.GetFileName(source)}的m4a音频…";
                        });
                        Cmd ccc = new Cmd();
                        var cmd = $" -y -i \"{source}\" -vn -codec copy -threads 2  \"{target}\"";
                        ccc.RunCmd(cmd);

                        this.Invoke((EventHandler)delegate
                        {
                            textStatus.Text = $"正在将{Path.GetFileName(source)}的m4a音频转化为MP3…";
                        });
                        ccc = new Cmd();
                        cmd = $" -y -i \"{target}\"  -threads 2 \"{target1}\"";
                        ccc.RunCmd(cmd);

                        File.Delete(target);
                    }
                    this.Invoke((EventHandler)delegate
                    {
                        textStatus.Text = "";
                    MessageBox.Show("全部提取完毕！");
                    });
                }
            })).Start();

        }

        private void btnToMp3_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                if (listBoxFiles.Items.Count > 0)
                {
                    var size = listBoxFiles.Items.Count;
                    for (int i = 0; i < size; i++)
                    {
                        Cmd ccc = new Cmd();
                        var path = listBoxFiles.Items[i].ToString();
                        var ext = Path.GetExtension(listBoxFiles.Items[0].ToString());
                        this.Invoke((EventHandler)delegate
                        {
                            textStatus.Text = $"正在转换{Path.GetFileName(path)}…";
                        });
                        var cmd = $" -y -i \"{path}\"  -threads 2 \"{path.Replace(ext, "")}.mp3\"";
                        ccc.RunCmd(cmd);
                    }
                    textStatus.Text = "";
                    MessageBox.Show("转换完毕！");
                }
            })).Start();
        }
        private void buttonToMp4_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                if (listBoxFiles.Items.Count > 0)
                {
                    var size = listBoxFiles.Items.Count;
                    for (int i = 0; i < size; i++)
                    {
                        Cmd ccc = new Cmd();
                        var path = listBoxFiles.Items[i].ToString();
                        var ext = Path.GetExtension(listBoxFiles.Items[0].ToString());
                        this.Invoke((EventHandler)delegate
                        {
                            textStatus.Text = $"正在转换{Path.GetFileName(path)}…";
                        });
                        var cmd = $" -y -i \"{path}\" -c:v copy -c:a copy -threads 2 \"{path.Replace(ext, "")}.mp4\"";
                        ccc.RunCmd(cmd);
                    }
                    textStatus.Text = "";
                    MessageBox.Show("转换完毕！");
                }
            })).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                if (listBoxFiles.Items.Count > 0)
                {
                    var ext = Path.GetExtension(listBoxFiles.Items[0].ToString());
                    for (int i = 0; i < listBoxFiles.Items.Count; i++)
                    {
                        var source = listBoxFiles.Items[i].ToString();
                        if (source.EndsWith(".m4a"))
                        {
                        var target = $"{ source.Replace(ext, "") }.mp3";
                        this.Invoke((EventHandler)delegate
                        {
                            textStatus.Text = $"正在将{Path.GetFileName(source)}转化为MP3…";
                        });
                        var ccc = new Cmd();
                        var cmd = $" -y -i \"{source}\"  -threads 2 \"{target}\"";
                        ccc.RunCmd(cmd);
                        }

                    }
                    this.Invoke((EventHandler)delegate
                    {
                        textStatus.Text = "";
                        MessageBox.Show("全部转化完毕！");
                    });
                }
            })).Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                if (listBoxFiles.Items.Count > 0)
                {
                    var ext = Path.GetExtension(listBoxFiles.Items[0].ToString());
                    for (int i = 0; i < listBoxFiles.Items.Count; i++)
                    {
                        var source = listBoxFiles.Items[i].ToString();
                        if (source.EndsWith(".mp3"))
                        {
                            var fi = new FileInfo(source);
                            var target = $"{fi.Directory}\\截取\\{fi.Name}";
                            this.Invoke((EventHandler)delegate
                            {
                                textStatus.Text = $"正在截取{Path.GetFileName(source)}…";
                            });
                            var ccc = new Cmd();
                            //ffmpeg.exe - i  audio提取版.mp3 - ss 00:00:00 - t  00:01:00  audio截取版.mp3
                             var cmd = $" -y  -i \"{source}\" - ss 00:00:08  -threads 2 \"{target}\"";
                            ccc.RunCmd(cmd);
                        }

                    }
                    this.Invoke((EventHandler)delegate
                    {
                        textStatus.Text = "";
                        MessageBox.Show("全部截取完毕！");
                    });
                }
            })).Start();
        }
    }
}

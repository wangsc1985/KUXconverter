using System;
using System.Diagnostics;
using System.IO;

namespace KUXconverter
{
    public class Cmd
    {
        
        private Process proc = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public Cmd()
        {
            proc = new Process();
        }
        public String RunCmd (string cmd)
        {
            try
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = "ffmpeg.exe";
                proc.StartInfo.Arguments = cmd;
                proc.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
                proc.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                proc.StartInfo.RedirectStandardInput = true;//false; //true;//接受来自调用程序的输入信息
                proc.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                proc.Start();//启动程序
                proc.BeginErrorReadLine();//开始异步读取
                proc.StandardInput.AutoFlush = true;
                string outStr = proc.StandardOutput.ReadToEnd();//获取cmd窗口的输出信息
                proc.WaitForExit();//等待程序执行完退出进程
                proc.Close();//关闭关闭进程
                proc.Dispose();//释放资源
                return outStr;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}

using jupyter.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace jupyter
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1(string Name, string Folder)
        {
            InitializeComponent();
            this.projectListItemName.Text = Name;
            this.projectListItemFolder.Text = Folder; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 打开项目文件夹
            var parent = ((StackPanel)((Button)sender).Parent).Parent;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var projectListItem = VisualTreeHelper.GetChild(parent, i) as TextBlock;
                if (projectListItem != null)
                {
                    if (projectListItem.Name == "projectListItemFolder")
                    {
                        string path = projectListItem.Text;
                        try
                        {
                            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                        }
                        catch (Exception ex)
                        {
                            App.DCbox.Name = ex.Message;
                            WindowsManager2<右下角累加通知>.Show(App.DCbox);
                        }
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string path = this.projectListItemFolder.Text;
            string cmd = $"/C code \"{path}\""; // "/C"命令将执行指定的命令行，然后关闭命令提示符窗口。如果你希望在执行完命令后保持命令提示符窗口打开，你应该使用"/K"参数代替"/C"
            App.DCbox.Name = cmd;
            WindowsManager2<右下角累加通知>.Show(App.DCbox);
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "cmd.exe";
                processStartInfo.Arguments = cmd;
                processStartInfo.CreateNoWindow = true; // 隐藏终端界面
                Process.Start(processStartInfo).WaitForExit();
            }
            catch (Exception ex)
            {
                App.DCbox.Name = ex.Message;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path = this.projectListItemFolder.Text;
            string cmd = $"python -m jupyter lab --port=6969 --ip=* --allow-root --notebook-dir=\"{path.Replace("\\", "\\\\")}\" \n";
            string cmd2 = "conda activate lab\n";
            App.DCbox.Name = cmd;
            WindowsManager2<右下角累加通知>.Show(App.DCbox);
            try
            {
                // 启动命令行进程
                Process process = new Process();
                process.StartInfo.FileName = "powershell.exe"; // 命令行解释器
                process.StartInfo.UseShellExecute = false; // 是否使用操作系统外壳程序
                process.StartInfo.RedirectStandardInput = true; // 是否重定向标准输入
                process.StartInfo.RedirectStandardOutput = true; // 是否重定向标准输出
                process.StartInfo.CreateNoWindow = false; // 是否创建窗口
                process.Start();

                // 模拟用户输入
                process.StandardInput.WriteLine(cmd2);
                process.StandardInput.WriteLine(cmd);

            }
            catch (Exception ex)
            {
                App.DCbox.Name = ex.Message;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string path = this.projectListItemFolder.Text;
            string cmd = $"python -m jupyter notebook --port=6969 --ip=* --allow-root --notebook-dir=\"{path.Replace("\\", "\\\\")}\" \n";
            string cmd2 = "conda activate lab\n";
            App.DCbox.Name = cmd;
            WindowsManager2<右下角累加通知>.Show(App.DCbox);
            try
            {
                // 启动命令行进程
                Process process = new Process();
                process.StartInfo.FileName = "powershell.exe"; // 命令行解释器
                process.StartInfo.UseShellExecute = false; // 是否使用操作系统外壳程序
                process.StartInfo.RedirectStandardInput = true; // 是否重定向标准输入
                process.StartInfo.RedirectStandardOutput = true; // 是否重定向标准输出
                process.StartInfo.CreateNoWindow = false; // 是否创建窗口
                process.Start();

                // 模拟用户输入
                process.StandardInput.WriteLine(cmd2);
                process.StandardInput.WriteLine(cmd);

            }
            catch (Exception ex)
            {
                App.DCbox.Name = ex.Message;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
        }
    }
}

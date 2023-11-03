using jupyter.util;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Text.Json;
using Path = System.IO.Path;
using Masuit.Tools.Hardware;
using Masuit.Tools.Win32;
using System.Net;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Drawing;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;


namespace jupyter
{
    /// <summary>
    /// 项目管理.xaml 的交互逻辑
    /// </summary>
    public partial class 项目管理 : Window
    {
        static Dictionary<string, string> winNotebookPathsDict = new Dictionary<string, string>();
        public 项目管理()
        {
            InitializeComponent();
            UserControl1 userControl = new("商务智能实验", @"C:\Users\Administrator\WPSDrive\205034987\WPS云盘\07 绛亽新学习\01 电子商务\05 20级大四上学期\商务智能实验\notebook");
            itemsControl.Items.Add(userControl);

            for (int i = 1; i <= 20; i++)
            {
                UserControl1 _ = new($"MyProject{i}", $"D:\\Gitrepo\\hi-windom\\winNotebook\\demo\\MyProject{i}");
                itemsControl.Items.Add(_);
            }

            // 启动命令行进程
            Process p = new Process();
            p.StartInfo.FileName = "powershell.exe"; // 命令行解释器
            p.StartInfo.UseShellExecute = false; // 是否使用操作系统外壳程序
            p.StartInfo.RedirectStandardInput = true; // 是否重定向标准输入
            p.StartInfo.RedirectStandardOutput = true; // 是否重定向标准输出
            p.StartInfo.CreateNoWindow = true; // 是否创建窗口
            p.Start();

            // 模拟用户输入
            p.StandardInput.WriteLine("conda --version");
            p.StandardInput.WriteLine("exit");
            p.StandardInput.AutoFlush = true;

            // 获取cmd窗口的输出信息
            string multiLineString = p.StandardOutput.ReadToEnd();
            string pattern = @"^conda\s*\d+\.\d+";

            MatchCollection matches = Regex.Matches(multiLineString, pattern, RegexOptions.Multiline);

            foreach (Match match in matches.Cast<Match>())
            {
                this.condaV.Text = match.Value;
            }
            // 等待程序执行完退出进程
            p.WaitForExit();
            p.Close();


            // 启动命令行进程
            p = new Process();
            p.StartInfo.FileName = "powershell.exe"; // 命令行解释器
            p.StartInfo.UseShellExecute = false; // 是否使用操作系统外壳程序
            p.StartInfo.RedirectStandardInput = true; // 是否重定向标准输入
            p.StartInfo.RedirectStandardOutput = true; // 是否重定向标准输出
            p.StartInfo.CreateNoWindow = true; // 是否创建窗口
            p.Start();

            // 模拟用户输入
            p.StandardInput.WriteLine("conda info -e");
            p.StandardInput.WriteLine("exit");
            p.StandardInput.AutoFlush = true;

            // 获取cmd窗口的输出信息
            multiLineString = p.StandardOutput.ReadToEnd();
            var _shell = WindowsManager<Shell>.Show(new object()); // 单实例
            // 获取shellTextBox文本框的引用
            TextBox? shellTextBox = _shell.FindName("shellTextBox") as TextBox;
            // 定位到 "# conda environments:" 下一行并删除该行及其前面的全部行
            var index = multiLineString.IndexOf("# conda environments:") + "# conda environments:\n#\n".Length;
            multiLineString = multiLineString.Substring(index);
            // 按行处理并记录唯一的 "*" 所在行的行号 defaultEnvNum
            int defaultEnvNum = -1;
            var lines = multiLineString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (line.Contains('*'))
                {
                    defaultEnvNum = Array.IndexOf(lines, line);
                    lines[defaultEnvNum] = line.Replace("*", " ");
                    break;
                }
            }

            shellTextBox.Text += $"\n\n{defaultEnvNum}";
            // 将 "*" 替换为空格
            multiLineString = multiLineString.Replace("*", " ");
            var defaultEnv = string.Empty;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line) && !line.StartsWith(" ")) // Ignore empty lines and lines starting with a space
                {
                    Match match = Regex.Match(line, @"(?<Name>^[a-zA-Z0-9_.-]+)\s+(?:\t+|\s+)*(?<Path>.*)$");
                    if (match.Success)
                    {
                        string name = match.Groups["Name"].Value;
                        string path = match.Groups["Path"].Value;
                        shellTextBox.Text += $"\n\n{name} : {path}"; //debug
                        if (i == defaultEnvNum)
                        {
                            defaultEnv = name;
                        }
                    }
                }
            }


                App.DCbox.Name = $"Default environment: {defaultEnv}";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);

            // 等待程序执行完退出进程
            p.WaitForExit();
            p.Close();

        }



        public class MyProjectList
        {
            // 定义数据类
            public string Name { get; set; }
            public string Folder { get; set; }
        }


        static void RecursiveSearch(string path, int depth)
        {
            // 最大递归深度为7
            if (depth > 7) return;

            // 遍历指定路径下的所有文件和文件夹
            foreach (var folder in Directory.GetDirectories(path))
            {
                if (Directory.Exists(folder)) // 添加文件夹存在性检查
                {
                    RecursiveSearch(folder, depth + 1); // 递归遍历子文件夹
                }
            }

            App.DCbox.Name = path;
            WindowsManager2<右下角累加通知>.Show(App.DCbox);

            // 检查当前文件夹是否为 ".winnotebook" 文件夹
            if (Path.GetFileName(path) == ".winnotebook")
            {
                // 将完整路径添加到字典中
                string parentFolderName = Path.GetDirectoryName(path);
                winNotebookPathsDict[parentFolderName] = path;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            string path = @"d:\"; // 指定要遍历的路径
            RecursiveSearch(path, 0); // 从0层开始遍历

            // 将字典转为json字符串
            string json = JsonSerializer.Serialize(winNotebookPathsDict);

            // 将json字符串写入文件
            File.WriteAllText(@"D:\Gitrepo\hi-windom\winNotebook\demo\MyProject1\.winnotebook\path_to_your_json_file.json", json);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var MFTfile = new DriveInfo("D:").EnumerateFiles();
            var FindFiles = MFTfile.Select(x => Path.GetFileName(x) == "ProjectSettings.json");
            foreach (var f in FindFiles)
            {
                MessageBox.Show(f.ToString());
            }
        }
    }
}

using jupyter.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
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
using System.Security.Principal;
using System.DirectoryServices;
using Masuit.Tools.Win32;

namespace jupyter
{
    /// <summary>
    /// 登录.xaml 的交互逻辑
    /// </summary>
    public partial class 登录 : Window
    {
        public string UserName;
        public string UserPassword;
        private Dictionary<string, string> UserDict = new() { { "sa", "694357845" }, { "liaoshanyi", "694357700" } };


        // 强依赖
        public void ShowToast(string text, TimeSpan? time = null)
        {
            Toast toast = new Toast();
            toast.Content = text;
            toast.Margin = new Thickness(0, 10, 0, 0);
            ToastPanel.Children.Add(toast);
            if (time == null)
            {
                toast.SetTimeClose(TimeSpan.FromSeconds(3));
            }
            else
            {
                toast.SetTimeClose(time.Value);
            }
        }
        public 登录()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
            List<string> ilist = new() { "海文东郑州", "海文东海口", "海文东文昌", "海文东东方" };
            Instance.ItemsSource = ilist;
            Instance.SelectedItem = "海文东郑州";
            this.Activate();
            Account.Text = App.config.AppSettings.Settings["DefaultUser"].Value;
        }

        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            App.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }

        async private void Login_Button(object sender, RoutedEventArgs e)
        {
            if (Account.Text.ToString() == "") { ShowToast("账号不可为空"); return; }
            if (!UserDict.ContainsKey(Account.Text.ToString())) { ShowToast("账号不存在"); Password.Password = ""; return; }
            if (Password.Password.ToString() == "") { ShowToast("密码不可为空"); return; }
            if (Password.Password.ToString() != UserDict[Account.Text.ToString()]) { ShowToast("密码错误"); Password.Password = ""; return; }
            root.IsEnabled = false;
            string? aaa = "";
            string eee = "";
            Task ppp = Task.Run(() => Tototo(ref aaa, ref eee));
            ppp.Wait();
            if (aaa != null)
            {
                this.DialogResult = Convert.ToBoolean(1);
                if (App.Current.MainWindow == null) App.Current.MainWindow = new MainWindow(false);
                App.Current.MainWindow.Show();
                this.Close();
                ppp.Dispose();
            }
            else if (eee != "")
            {
                App.DCbox.Name = eee;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
                eee = "";
            }
            else
            {
                ShowToast("连接超时,请检查配置");
            }
            root.IsEnabled = true;
        }

        private void 使用Windows身份验证_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            root.IsEnabled = false;
            string eee = "";
            // 获取当前Windows用户
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();

            // 判断用户是否已登录
            if (currentIdentity != null)
            {
                // 获取Windows用户principal
                WindowsPrincipal principal = new WindowsPrincipal(currentIdentity);

                // 获取用户信息
                string username = currentIdentity.Name;
                string[] roles = { "Administrators", "Users" };

                // 判断用户是否具有指定的角色或权限
                foreach (string role in roles)
                {
                    if (principal.IsInRole(role))
                    {
                        if (username == App.config.AppSettings.Settings["TrustedUser"].Value)
                        {
                            eee = $"欢迎 {username} （属于 {role} 角色）";
                            Console.WriteLine(eee);
                            this.DialogResult = Convert.ToBoolean(1);
                            App.DCbox.Name = eee;
                            WindowsManager2<右下角累加通知>.Show(App.DCbox);
                            if (App.Current.MainWindow == null) App.Current.MainWindow = new MainWindow(false);
                            App.Current.MainWindow.Show();
                            this.Close();
                            break;
                        }
                        else
                        {
                            eee = $"无法为当前账号 {username} 提供免密登录";
                            Console.WriteLine(eee);
                            App.DCbox.Name = eee;
                            WindowsManager2<右下角累加通知>.Show(App.DCbox);
                            break;
                        }
                    }
                }
            }
            else
            {
                eee = "当前用户未登录。";
                Console.WriteLine(eee);
                App.DCbox.Name = eee;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            root.IsEnabled = true;
        }

        void Tototo(ref string? aaa, ref string eee)
        {
            try
            {
                Windows.ClearMemorySilent(); // 类似于各大系统优化软件的加速球功能
            }
            catch (Exception ex)
            {
                eee = ex.Message;
                aaa = null;
            }

        }

        private void Account_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account.Text == "liaoshanyi")
            {
                waitingAImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/qq.png"));
            } else if (Account.Text == "sa") {
                waitingAImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/image.jpg"));
            } else
            {
                waitingAImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/jupyter.png"));
            }
        }

        //“登录遇到问题”TextBlock触发事件
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowsManager<微信公众号>.Show(new object());
        }
    }
}

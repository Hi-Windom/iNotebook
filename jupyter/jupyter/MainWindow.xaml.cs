using jupyter.util;
using Masuit.Tools.Files;
using Masuit.Tools.Logging;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _AllowClose = false;
        bool _ShowingDialog = false;
        bool _reLogin = false;
        WindowsManager2_SingleStringArgsDC dc = new();
        public MainWindow(bool newin = true)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            #region 登录界面加载及验证
            //显示登陆界面，验证后返回。
            if (newin)
            {
                连接的实例.Text = "";
                登录 loginWindow = new();
                loginWindow.ShowDialog();
                if (loginWindow.DialogResult != Convert.ToBoolean(1))
                {
                    this.Hide();
                }
            }
            //显示登陆界面 结束
            #endregion
            this.Activate();
            App.Current.MainWindow = this;

        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.Activate();
            //If the user has elected to allow the close, simply let the closing event happen.
            if (_AllowClose || _reLogin) return;

            //NB: Because we are making an async call we need to cancel the closing event
            e.Cancel = true;

            //we are already showing the dialog, ignore
            if (_ShowingDialog) return;

            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(24);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = "确认要关闭主窗口并退出应用吗？其他窗口也会被关闭，操作不可撤销~";

            Button btn1 = new Button();
            Style style = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn1.Style = style;
            btn1.Width = 115;
            btn1.Height = 30;
            btn1.Margin = new Thickness(2);
            btn1.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn1.CommandParameter = true;
            btn1.Content = "是";

            Button btn2 = new Button();
            Style style2 = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn2.Style = style2;
            btn2.Width = 115;
            btn2.Height = 30;
            btn2.Margin = new Thickness(2);
            btn2.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn2.CommandParameter = false;
            btn2.Content = "否";


            DockPanel dck = new DockPanel();
            dck.Children.Add(btn1);
            dck.Children.Add(btn2);

            StackPanel stk = new StackPanel();
            stk.Width = 230;
            stk.Height = 180;
            stk.Children.Add(txt1);
            stk.Children.Add(dck);

            //Set flag indicating that the dialog is being shown
            _ShowingDialog = true;
            object result = await MaterialDesignThemes.Wpf.DialogHost.Show(stk);
            _ShowingDialog = false;
            //The result returned will come form the button's CommandParameter.
            //If the user clicked "Yes" set the _AllowClose flag, and re-trigger the window Close.
            if (result is bool boolResult && boolResult)
            {
                _AllowClose = true;
                App.Taskbar.Icon = null;
                App.Taskbar.Dispose();
                Application.Current.Shutdown(); Process.GetCurrentProcess().Kill();
                //Close();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _reLogin = true;
            //foreach (Window w in Application.Current.Windows) w.Close();
            MyWindowClass.隐藏非指定窗口(string.Empty);
            登录 dl = new();
            dl.ShowDialog();
            _reLogin = false;
            this.Activate();
            App.Current.MainWindow = this;
        }
        private void 项目管理_Click(object sender, RoutedEventArgs e)
        {
            WindowsManager<项目管理>.Show(new object()); // 单实例
            //this.Hide();
        }
        private void 智能任务_Click(object sender, RoutedEventArgs e)
        {
            //WindowsManager<智能任务>.Show(new object()); // 单实例
            //this.Hide();
        }
        private void 控制中心_Click(object sender, RoutedEventArgs e)
        {
            WindowsManager<控制中心>.Show(new object()); // 单实例
            //this.Hide();
        }
        private void 关于软件_Click(object sender, RoutedEventArgs e)
        {
            WindowsManager<关于软件>.Show(new object()); // 单实例
            //this.Hide();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 窗口拖动
            try
            {
                DragMove();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void dev1_Click(object sender, RoutedEventArgs e)
        {
            LogManager.LogDirectory = AppDomain.CurrentDomain.BaseDirectory + "/logs";
            LogManager.Event += info =>
            {
                //todo:注册一些事件操作
                App.DCbox.Name = LogManager.LogDirectory;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            };
            LogManager.Info("记录一次消息");
            LogManager.Error(new Exception("异常消息"));
        }

        private void dev2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dev3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void logEntrance_Click(object sender, RoutedEventArgs e)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "logs";

            try
            {
                this.logEntrance.IsEnabled = false;
                Process.Start("explorer.exe", path);
                App.DCbox.Name = $"{path} 已就绪，首次打开可能存在延时，如已打开请检查资源管理窗口";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            catch (Exception ex)
            {
                if (!System.IO.Directory.Exists(path))
                {
                    MessageBox.Show($"The directory {path} does not exist.");
                    return;
                }
                App.DCbox.Name = ex.Message;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            this.logEntrance.IsEnabled = true;
        }
    }
}
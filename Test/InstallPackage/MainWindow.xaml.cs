using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.IO.Compression;
using System.Resources;
using System.ComponentModel;
using Installationpackage.Helper;
using Installationpackage.Model;
using Microsoft.Practices.ServiceLocation;

namespace Installationpackage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public BackgroundWorker bg { get; set; }

        public ProgressModel Progress { get; set; }

        //是否退出的标志位
        private bool Exit = false;

        public MainWindow()
        {
            ServiceLocator.SetLocatorProvider(() => IocHelper.SimpleIOC);

            InitializeComponent();

            Progress = ModelLocator.Locator.Progress;

            this.bg = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            this.bg.DoWork += this.Bg_DoWork;
            this.bg.ProgressChanged += this.Bg_ProgressChanged;
            this.bg.RunWorkerCompleted += this.Bg_RunWorkerCompleted;
        }

        private BackgroundWorker BGInit()
        {
            return null;
#if false
            this.bg = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            this.bg.DoWork += this.Bg_DoWork;
            this.bg.ProgressChanged += this.Bg_ProgressChanged;
            this.bg.RunWorkerCompleted += this.Bg_RunWorkerCompleted;
#endif
        }

        //关闭窗体
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //最小化窗体
        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //窗体移动
        private void TitleGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        //快速安装
        private void QucikInstallBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Exit)
            {
                this.Close();
                return;
            }

            //清除之前的进度
            Progress.Progress = 0;

            this.bg.RunWorkerAsync();
            Task task = new Task(() =>
            {
                Progress.Level = 1;
                //创建安装目录,确保目录存在
                if (!Directory.Exists(SetupPath))
                    Directory.CreateDirectory(SetupPath);

                Progress.Level = 2;
                //已存在文件,清空文件夹后重新安装
                if (Directory.GetDirectories(SetupPath).Length != 0)
                {
                    Directory.Delete(SetupPath, true);
                    System.Threading.Thread.Sleep(500);
                    Directory.CreateDirectory(SetupPath);
                }

                //释放嵌入的Zip资源
                ResourcesHelper.ExtractResFile("Installationpackage.Resources.Debug.zip", SetupPath + "\\Debug.zip");

                Progress.Level = 3;
                //将Zip资源解压
                ZipFile.ExtractToDirectory(SetupPath + "\\Debug.zip", SetupPath);

                Progress.Level = 4;
                //删除使用后的Zip资源
                if (System.IO.File.Exists(SetupPath + "\\Debug.zip"))
                {
                    System.IO.File.Delete(SetupPath + "\\Debug.zip");
                }
                Progress.Level = 5;
                //创建快捷方式
                CreateLink();
            });
            task.Start();
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                Exit = true;
                QucikInstallBtn.Content = "退出";
                QucikInstallBtn.Visibility = Visibility.Visible;
                InstallProgress.Visibility = Visibility.Collapsed;
            }));
        }

        private void Bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.InstallProgress.Value = (double)e.ProgressPercentage;
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                QucikInstallBtn.Visibility = Visibility.Collapsed;
                InstallProgress.Visibility = Visibility.Visible;
            }));
            while (true)
            {
                if (this.Progress.Progress < this.Progress.Level * 20)
                {
                    this.Progress.Progress += 2;
                }
                this.bg.ReportProgress(this.Progress.Progress);
                if (this.Progress.Progress == 100)
                {
                    break;
                }
                System.Threading.Thread.Sleep(30);
            }
        }

        //创建快捷方式
        private string SetupPath = @"D:\BodorThinker5.0";

        private void CreateLink()
        {
            WshShell shell = new WshShell();
            IWshShortcut wshShortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//BodorThinker5.0.lnk");//
            wshShortcut.TargetPath = SetupPath + "\\BodorThinker5.0.exe";
            wshShortcut.WorkingDirectory = SetupPath;
            wshShortcut.WindowStyle = 1;
            wshShortcut.Description = "BodorThinker5.0";
            wshShortcut.Save();
        }
    }
}

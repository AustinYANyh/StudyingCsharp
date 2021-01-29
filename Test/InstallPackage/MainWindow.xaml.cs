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
using System.Resources;
using System.ComponentModel;
using BodorThinkerInstallHelper.Helper;
using BodorThinkerInstallHelper.Model;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using Ionic.Zip;
using System.IO.Packaging;
using System.Windows.Markup;
using System.Windows.Interop;

namespace BodorThinkerInstallHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public BackgroundWorker bg { get; set; }

        public MainModel Main { get; set; }

        public ProgressModel Progress { get; set; }

        //是否处于更多模式的标志位
        private bool IsMore = false;

        public MainWindow()
        {
            ServiceLocator.SetLocatorProvider(() => IocHelper.SimpleIOC);

            //ResourceDictionary resourceDictionary = new ResourceDictionary();
            //resourceDictionary.Source = new Uri("pack://application:,,,/BodorThinkerInstall;component/Dictionary.xaml", UriKind.RelativeOrAbsolute);
            //this.Resources.MergedDictionaries.Add(resourceDictionary);

            //InitializeComponent();
            ResourcesHelper.LoadViewFromUri(this, "/BodorThinkerInstallHelper;component/MainWindow.xaml");

            var bitmap = BodorThinkerInstallHelper.Properties.Resources.SplashScreen;
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                                                                          IntPtr.Zero,
                                                                          Int32Rect.Empty,
                                                                          BitmapSizeOptions.FromEmptyOptions()
          );
            bitmap.Dispose();
            var brush = new ImageBrush(bitmapSource);
            BackGroundIMG.Fill = brush;


            var newbitmap = BodorThinkerInstallHelper.Properties.Resources.More;
            var newbitmapSource = Imaging.CreateBitmapSourceFromHBitmap(newbitmap.GetHbitmap(),
                                                                                     IntPtr.Zero,
                                                                                     Int32Rect.Empty,
                                                                                     BitmapSizeOptions.FromEmptyOptions()
            );

            newbitmap.Dispose();
            var newbrush = new ImageBrush(newbitmapSource);
            MoreBtn.Fill = newbrush;
            Main = ModelLocator.Locator.Main;
            Progress = ModelLocator.Locator.Progress;

            this.DataContext = this;

            this.bg = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            this.bg.DoWork += this.Bg_DoWork;
            this.bg.ProgressChanged += this.Bg_ProgressChanged;
            this.bg.RunWorkerCompleted += this.Bg_RunWorkerCompleted;
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
            System.Windows.Application.Current.MainWindow.DragMove();
        }

        //更多
        private void MoreBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMore)
            {
                //SetUpPathLabel.Visibility = Visibility.Collapsed;
                //SetUpPathBox.Visibility = Visibility.Collapsed;

                var QuickBtnDown = (Storyboard)this.FindResource("QuickBtnDown");
                var MoreBtnDown = (Storyboard)this.FindResource("MoreBtnDown");
                var ParaBackUpCheckBoxDown = (Storyboard)this.FindResource("ParaBackUpCheckBoxDown");
                var SetUpPathShowOff = (Storyboard)this.FindResource("SetUpPathShowOff");
                QuickBtnDown.Begin();
                MoreBtnDown.Begin();
                ParaBackUpCheckBoxDown.Begin();
                SetUpPathShowOff.Begin();
                IsMore = false;
            }
            else
            {
                var QuickBtnUp = (Storyboard)this.FindResource("QuickBtnUp");
                var MoreBtnUp = (Storyboard)this.FindResource("MoreBtnUp");
                var ParaBackUpCheckBoxUp = (Storyboard)this.FindResource("ParaBackUpCheckBoxUp");
                var SetUpPathShow = (Storyboard)this.FindResource("SetUpPathShow");
                QuickBtnUp.Begin();
                MoreBtnUp.Begin();
                ParaBackUpCheckBoxUp.Begin();
                SetUpPathShow.Begin();

                //SetUpPathLabel.Visibility = Visibility.Visible;
                //SetUpPathBox.Visibility = Visibility.Visible;
                IsMore = true;
            }
        }

        //路径选择
        private void PathSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Main.SetUpPath = folderBrowserDialog.SelectedPath;
            }
        }

        //退出
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            //安装完毕,关闭窗体
            this.Close();
        }

        //快速安装
        private void QucikInstallBtn_Click(object sender, RoutedEventArgs e)
        {
            //使用打包携带的参数
            if (!LocalParaCheckBox.IsChecked.Value)
            {
                Progress.Message = "正在执行安装前动作...";

                //参数文件使用覆盖解压的方式,不清除用户文件夹下其他文件
                using (ZipFile zip = new ZipFile(Main.DataZipFilePath))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        entry.Extract(Main.DataPath, ExtractExistingFileAction.OverwriteSilently);
                        Progress.Message = string.Join(" ", "正在释放资源:", entry.FileName);
                    }
                }

                System.IO.File.Delete(Main.DataZipFilePath);
            }

            //清除之前的进度
            Progress.Progress = 0;

            Task task = new Task(() =>
            {
                Progress.Level = 1;
                //创建安装目录,确保目录存在
                if (!Directory.Exists(Main.SetUpPath))
                    Directory.CreateDirectory(Main.SetUpPath);

                Progress.Level = 3;
                //将Zip资源解压
                using (ZipFile zip = new ZipFile(Main.InstallZipFilePath))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        entry.Extract(Main.SetUpPath, ExtractExistingFileAction.OverwriteSilently);
                        Progress.Message = string.Join(" ", "正在释放资源:", entry.FileName);
                    }
                }

                Progress.Level = 4;
                Progress.Message = "正在执行安装后清理工作...";
                //删除使用后的Zip资源
                if (System.IO.File.Exists(Main.InstallZipFilePath))
                {
                    System.IO.File.Delete(Main.InstallZipFilePath);
                }
                //删除存在的bdData版本
                if (System.IO.File.Exists(System.IO.Path.Combine(Main.SetUpPath,"bdData.zip")))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(Main.SetUpPath, "bdData.zip"));
                }

                //删除资源临时文件夹
                if (System.IO.Directory.Exists(Main.TmpZipFilePath))
                    System.IO.Directory.Delete(Main.TmpZipFilePath, true);

                Progress.Level = 5;
                Progress.Message = "正在为程序创建快捷方式...";
                //创建快捷方式
                CreateLink();
            });
            task.Start();
            this.bg.RunWorkerAsync();
        }

        //覆盖压缩
        private void ExtractCover(string SourcePath, string DestPath)
        {
            if (!Directory.Exists(DestPath))
                Directory.CreateDirectory(DestPath);

            string TmpPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "tmp");
            if (!Directory.Exists(TmpPath))
                Directory.CreateDirectory(TmpPath);

            using (ZipFile zip = new ZipFile(SourcePath))
            {
                zip.ExtractAll(TmpPath);
            }

            string[] FileNameList = Directory.GetFiles(System.IO.Path.Combine(TmpPath, "bdData"), "*.xml", SearchOption.AllDirectories);

            //处理文件
            foreach (string file in FileNameList)
            {
                FileInfo info = new FileInfo(file);

                int index = info.FullName.IndexOf(@"bdData\");
                string name = info.FullName.Substring(index + @"bdData\".Length);

                //文件存在，覆盖掉
                if (System.IO.File.Exists(System.IO.Path.Combine(DestPath, name)))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(DestPath, name));
                    System.IO.File.Move(info.FullName, System.IO.Path.Combine(DestPath, name));
                }
                else
                {
                    System.IO.File.Move(info.FullName, System.IO.Path.Combine(DestPath, name));
                }
            }
            Directory.Delete(TmpPath, true);
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                InstallProgress.Visibility = Visibility.Collapsed;
                MoreBtn.Visibility = Visibility.Collapsed;

                ExitBtn.Visibility = Visibility.Visible;
                ProgressMessage.Visibility = Visibility.Collapsed;
            }));
        }

        private void Bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.InstallProgress.Value = (double)e.ProgressPercentage;
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            //开始安装前动作,按钮不可见
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                MoreBtn.Visibility = Visibility.Collapsed;
                QucikInstallBtn.Visibility = Visibility.Collapsed;
                InstallProgress.Visibility = Visibility.Visible;
                LocalParaCheckBox.Visibility = Visibility.Collapsed;
                ProgressMessage.Visibility = Visibility.Visible;

                if (IsMore)
                {
                    var SetUpPathShowOff = (Storyboard)this.FindResource("SetUpPathShowOff");
                    SetUpPathShowOff.Begin();
                }
            }));

            //进度条
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
        private void CreateLink()
        {
            WshShell shell = new WshShell();
            IWshShortcut wshShortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//BodorThinker5.0.lnk");//
            wshShortcut.TargetPath = Main.SetUpPath + "\\BodorThinker5.0.exe";
            wshShortcut.WorkingDirectory = Main.SetUpPath;
            wshShortcut.WindowStyle = 1;
            wshShortcut.Description = "BodorThinker5.0";
            wshShortcut.Save();
        }
    }
}

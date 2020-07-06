using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace testLineAttritube
{
    public class MainWindowViewModel : NotificationObject
    {
        public DelegateCommand MinimalCommand { get; set; }
        public DelegateCommand MaxmalCommand { get; set; }
        public DelegateCommand CloseWindowCommand { get; set; }
        public DelegateCommand DragMoveCommand { get; set; }
        public MainWindowViewModel()
        {
            #region 标题栏命令

            MinimalCommand = new DelegateCommand(() => Application.Current.MainWindow.WindowState = WindowState.Minimized);

            MaxmalCommand = new DelegateCommand(() =>
                Application.Current.MainWindow.WindowState =
                Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);

            CloseWindowCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());

            DragMoveCommand = new DelegateCommand(() => Application.Current.MainWindow.DragMove());

            #endregion
        }
    }
}

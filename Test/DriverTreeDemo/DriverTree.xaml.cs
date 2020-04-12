using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DriverTreeDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DriveInfo[] driveInfos = DriveInfo.GetDrives();

            foreach (var each in driveInfos)
            {
                DriveInfos.Items.Add("本地磁盘(" + each.Name.Replace("\\", "") + ")");
            }

            if (DriveInfos.Items.Count != 0)
            {
                DriveInfos.SelectedItem = DriveInfos.Items[0];
                //Init_TreeView();
            }
        }

        private void InitTree()
        {
            FileTree.Items.Add(DriveInfos.SelectedItem);

            string root = DriveInfos.SelectedItem.ToString().Substring(5, 2) + @"\\";
            try
            {
                DirectoryInfo directory = new DirectoryInfo(root);
                DirectoryInfo[] filesdirectory = directory.GetDirectories();

                var filtered = filesdirectory.Where(f => !f.Attributes.HasFlag(FileAttributes.System));

                //添加文件夹
                foreach (var f in filtered)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = f.Name;
                    item.FontSize = 21;
                    FileTree.Items.Add(item);

                    item.Selected += Each_Selected;
                    //SetStyle(item);
                }

                for (int i = 1; i < FileTree.Items.Count; ++i)
                {
                    TreeViewItem current = FileTree.Items[i] as TreeViewItem;
                    string path = root + current.Header.ToString() + @"\\";
                    directory = new DirectoryInfo(path);
                    filesdirectory = directory.GetDirectories();

                    filtered = filesdirectory.Where(f => !f.Attributes.HasFlag(FileAttributes.System));

                    //添加第二层文件夹
                    foreach (var f in filtered)
                    {
                        TreeViewItem item = new TreeViewItem();
                        item.Header = f.Name;
                        item.FontSize = 21;
                        current.Items.Add(item);

                        item.Selected += Each_Selected;
                        //SetStyle(item);
                    }
                }
            }
            catch (Exception error)
            {
               
            }
        }

        private void Each_Selected(object sender, RoutedEventArgs e)
        {
            //磁盘数据添加
            TreeViewItem current = FileTree.SelectedItem as TreeViewItem;

            if (current == null)
                return;

            string root = DriveInfos.SelectedItem.ToString().Substring(5, 2) + @"\";
            string path = current.Header.ToString();

            while ((current.Parent as TreeViewItem) != null)
            {
                current = current.Parent as TreeViewItem;
                path = current.Header.ToString() + @"\" + path;
            }

            current = FileTree.SelectedItem as TreeViewItem;

            SetData(current, root, path);

            //单击闭合
            current.IsExpanded = !current.IsExpanded;
            current.IsSelected = false;
            e.Handled = true;

            //调节滚动条
            TreeViewAutomationPeer lvap = new TreeViewAutomationPeer(FileTree);
            var svap = lvap.GetPattern(PatternInterface.Scroll) as ScrollViewerAutomationPeer;
            var scroll = svap.Owner as ScrollViewer;

            if (current.Items.Count > 21)   //TO DO(待完善)
            {
                int count = GetCountOfTree(current);
                scroll.ScrollToVerticalOffset(0);
                scroll.ScrollToVerticalOffset((count + 1) * 25);//25目前是item的高度
            }

        }
        private async void SetData(TreeViewItem current, string root, string path)
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    for (int i = 0; i < current.Items.Count; ++i)
                    {
                        TreeViewItem subitem = current.Items[i] as TreeViewItem;
                        string tmpPath = path + @"\" + subitem.Header.ToString() + @"\";

                        if (subitem.HasItems == false)
                        {
                            try
                            {
                                DirectoryInfo directory = new DirectoryInfo(root + tmpPath);
                                DirectoryInfo[] filesdirectory = directory.GetDirectories();

                                var filtered = filesdirectory.Where(f => !f.Attributes.HasFlag(FileAttributes.System));

                                //添加文件夹
                                foreach (var f in filtered)
                                {
                                    TreeViewItem item = new TreeViewItem();
                                    item.Header = f.Name;
                                    item.FontSize = 21;
                                    subitem.Items.Add(item);
                                    //SetStyle(item);
                                }
                            }
                            catch (Exception error)
                            {
                               
                            }
                        }
                    }
                }));
            });
        }

        private int GetCountOfTree(TreeViewItem item)
        {
            for (int i = 0; i < FileTree.Items.Count; ++i)
            {
                if (FileTree.Items[i] == item)
                {
                    return i + 1;
                }
                else
                {
                    continue;
                }
            }
            return -1;
        }

        private void SetStyle(TreeViewItem item)
        {
            Style style = new Style();
            style = (Style)FindResource("TreeViewItemStyle");
            item.Style = style;
        }

        private void DriveInfos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileTree.Items.Clear();
            InitTree();
        }
    }
}

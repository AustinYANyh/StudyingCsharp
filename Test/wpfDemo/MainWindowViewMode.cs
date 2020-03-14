using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace studyWPF_1.viewMode
{
    class MainWindowViewMode: BindableBase
    {
        public class Friend
        {
            public string NickName { get; set; }
            public BitmapImage Head { get;set; }
        }

        public DelegateCommand<object> SelectItemChangedCommand { get; set; }
        public DelegateCommand<object> closeCommand { get; set; }
        public MainWindowViewMode()
        {
            friends = new ObservableCollection<Friend>();
            friends.Add(new Friend() { NickName = "染墨灬若流云", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/head1.jpg")) });
            friends.Add(new Friend() { NickName = "执笔灬绘浮沉", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/head2.jpg")) });
            friends.Add(new Friend() { NickName = "素手灬挽秋风", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/github.png")) });          

            SelectItemChangedCommand = new DelegateCommand<object>((p) =>
            {
                ListView lv = p as ListView;
                Friend friend = lv.SelectedItem as Friend;
                Head = friend.Head;
                NickName = friend.NickName;
            });

            closeCommand = new DelegateCommand<object>((p) =>
            {
                Application.Current.Shutdown();
            });
        }

        private ObservableCollection<Friend> friends;

        public ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }
            set
            {
                friends = value;
            }
        }

        private BitmapImage headPic;

        public BitmapImage Head
        {
            get
            {
                return headPic;
            }
            set
            {
                SetProperty(ref headPic, value);
            }
        }

        private string nickname;

        public string NickName
        {
            get
            {
                return nickname;
            }
            set
            {
                SetProperty(ref nickname, value);
            }
        }
    }
}

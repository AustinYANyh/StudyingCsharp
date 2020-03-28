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
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace studyWPF_1.viewMode
{
    class MainWindowViewMode : BindableBase
    {
        public class Friend
        {
            public string NickName { get; set; }
            public BitmapImage Head { get; set; }
        }

        public class itemValue
        {
            public string Value { get; set; }
        }
        
        public class Message
        {
            public string SendMessage { get; set; }
        }

        public DelegateCommand<object> SelectItemChangedCommand { get; set; }
        public DelegateCommand<object> closeCommand { get; set; }
        public DelegateCommand<object> send_Click { get; set; }

        public MainWindowViewMode()
        {
            friends = new ObservableCollection<Friend>();
            friends.Add(new Friend() { NickName = "染墨灬若流云", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/head1.jpg")) });
            friends.Add(new Friend() { NickName = "执笔灬绘浮沉", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/head2.jpg")) });
            friends.Add(new Friend() { NickName = "素手灬挽秋风", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/github.png")) });

            values = new ObservableCollection<itemValue>();
            values.Add(new itemValue() { Value = "Orange" });
            values.Add(new itemValue() { Value = "Apple" });
            values.Add(new itemValue() { Value = "Pear" });

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

            messages = new ObservableCollection<Message>();

            send_Click = new DelegateCommand<object>((p) =>
            {
                RichTextBox messageBox = p as RichTextBox;
                string message = new TextRange(messageBox.Document.ContentStart, messageBox.Document.ContentEnd).Text;
                messageBox.Document.Blocks.Clear();

                messages.Add(new Message() { SendMessage = message });
            });
        }

        private ObservableCollection<Friend> friends;
        private ObservableCollection<itemValue> values;

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

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
            }
        }
        public ObservableCollection<itemValue> itemValues
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
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

        private string sendMessage;

        public string SendMessage
        {
            get
            {
                return sendMessage;
            }
            set
            {
                sendMessage = value;
            }
        }

        public string itemvalue;

        public string Value
        {
            get
            {
                return itemvalue;
            }
            set
            {
                SetProperty(ref itemvalue, value);
            }
        }
    }
}

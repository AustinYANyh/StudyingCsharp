using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testListBox.ViewMode
{
    class MainWindowViewMode
    {
        public class LogMessage
        {
            public string Time { get; set; }
            public string Message { get; set; }
            public int MessageType { get; set; }
        }

        public MainWindowViewMode()
        {
            logMessages = new ObservableCollection<LogMessage>();

            logMessages.Add(new LogMessage() { Time = DateTime.Now.ToString("MM/dd HH:mm:ss"), Message = "正在操作...", MessageType = 0 });
            logMessages.Add(new LogMessage() { Time = DateTime.Now.ToString("MM/dd HH:mm:ss"), Message = "操作结束...", MessageType = 1 });
            logMessages.Add(new LogMessage() { Time = DateTime.Now.ToString("MM/dd HH:mm:ss"), 
                Message = "当前屏幕分辨率为1920 × 1080", MessageType = 2 });
        }

        private ObservableCollection<LogMessage> logMessages;

        public ObservableCollection<LogMessage> LogMessages
        {
            get
            {
                return logMessages;
            }
            set
            {
                logMessages = value;
            }
        }

        private string time;

        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        public string message
        {
            get
            {
                return message;
            }
            set
            {
                value = message;
            }
        }

        private int messagetype;

        public int MessageType
        {
            get
            {
                return messagetype;
            }
            set
            {
                messagetype = value;
            }
        }
    }
}

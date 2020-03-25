using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testRichTextBox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public class LogMessage
        {
            public string Time { get; set; }
            public string Message { get; set; }
            public string MessageType { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            string MessageType = "";

            LogMessage logMessage = new LogMessage();
            logMessage.Time = DateTime.Now.ToString("(MM/dd hh:mm:ss) ");
            logMessage.Message = "Enter Module: WorkBase";
            logMessage.MessageType = "进入模块";

            string normalLogDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BodorCut\\NormalLog\\";
            string normalLogFileName = DateTime.Now.ToString("yyyyMMdd") + ".Normal.txt";

            StreamReader sr = new StreamReader(normalLogDir + normalLogFileName, Encoding.Default);

            string Line;
            while ((Line = sr.ReadLine()) != null)
            {
                string message = "";  string messageType = ""; string time = "";
                getDetial(Line, ref time, ref messageType, ref message);

                FlowDocument flowDocument = new FlowDocument();
                Paragraph paragraph = new Paragraph();
                Run run = new Run();

                if (time != "")
                {
                    run = new Run(time + " ");
                    paragraph.Inlines.Add(run);
                }

                MessageType = messageType;
                

                switch (MessageType)
                {
                    case "进入模块":
                        run = new Run(message);
                        run.Foreground = new SolidColorBrush(Colors.Green);
                        paragraph.Inlines.Add(run);
                        break;
                    case "普通":
                        run = new Run(message);
                        run.Foreground = new SolidColorBrush(Colors.Black);
                        paragraph.Inlines.Add(run);
                        break;
                    case "报警信息":
                        run = new Run(message);
                        run.Foreground = new SolidColorBrush(Colors.Red);
                        paragraph.Inlines.Add(run);
                        break;
                    case "坐标相关":
                        run = new Run(message);
                        run.Foreground = new SolidColorBrush(Colors.Blue);
                        paragraph.Inlines.Add(run);
                        break;
                }
                richtextbox.Document.Blocks.Add(paragraph);
            }
        }

        public void getDetial(string Line, ref string time, ref string messageType, ref string message)
        {
            try
            {
                if (Line.IndexOf(" ") != -1)
                {
                    time = Line.Substring(0, Line.IndexOf(")") + 1);
                    Line = Line.Substring(Line.IndexOf(")") + 2);
                    messageType = Line.Substring(0, Line.IndexOf(" "));
                    Line = Line.Substring(Line.IndexOf(" ") + 1);
                    message = Line;
                }
                else
                {
                    messageType = "普通";
                    message = Line;
                }
            }
            catch (Exception error)
            {
                //图形类的信息,类型后直接换行,不存在空格,indexof出错,直接返回
                return;
            }
        }

        private void click_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Log Files|*.rtf";

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                FileStream fileStream = new FileStream(path, FileMode.Create);
                //StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

                TextRange textRange = new TextRange(richtextbox.Document.ContentStart, richtextbox.Document.ContentEnd);
                textRange.Save(fileStream, System.Windows.DataFormats.Rtf);

                //streamWriter.Flush();
                //streamWriter.Close();
                fileStream.Close();
            }
        }
    }
}

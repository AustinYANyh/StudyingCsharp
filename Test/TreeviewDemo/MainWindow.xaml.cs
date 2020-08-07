using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace testTreeBinding
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InputData();
            tree.ItemsSource = getNodes(0, nodes);
        }

        private List<TreeNode> getNodes(int parentID, List<TreeNode> nodes)
        {
            List<TreeNode> mainNodes = nodes.Where(x => x.ParentID == parentID).ToList();
            List<TreeNode> otherNodes = nodes.Where(x => x.ParentID != parentID).ToList();
            foreach (TreeNode node in mainNodes)
                node.ChildNodes = getNodes(node.NodeID, otherNodes);
            return mainNodes;
        }

        private List<TreeNode> nodes;
        private void InputData()
        {
            List<string> logFileList = GetLogFileList();
            List<string> date = new List<string>();
            List<string> NormalDate = new List<string>();

            for (int i = logFileList.Count - 1; i >= 0; i--)
            {
                string tmp = logFileList[i].Substring(0, 6);
                NormalDate.Add(tmp);
                tmp = tmp.Insert(4, "-");
                date.Add(tmp);

            }
            date = date.Distinct().ToList();
            NormalDate = NormalDate.Distinct().ToList();

            List<string> timeList = new List<string>();
            List<string> dateList = new List<string>();
            List<string> lauchTimeList = new List<string>();
            nodes = new List<TreeNode>();
            int nodecount = nodes.Count;
            for (int i = 0; i < date.Count; ++i)
            {
                TreeNode node = new TreeNode { ParentID = 0, NodeID = i + 1, NodeName = date[i] };
                nodes.Add(node);

                List<string> fileList = logFileList.FindAll((p) => p.IndexOf(NormalDate[i]) != -1);
                for (int j = fileList.Count - 1; j >= 0; j--)
                {
                    string logText = GetLogText(System.IO.Path.Combine("D:\\bdData\\logs", fileList[j]));

                    string pattern = @"\S\d{1,2}\/\d{1,2} \d{1,2}\:\d{2}\:\d{2}\S\s\w{2,4}\s------Logger Start------";

                    GetDateAndLanuchTime(timeList, ref dateList, lauchTimeList, logText, pattern);

                    //添加二级菜单
                    string day = fileList[j].Substring(0, fileList[j].IndexOf("."));
                    day = day.Substring(6);
                    StringBuilder stringBuilder = new StringBuilder(date[i]);
                    stringBuilder.Append("-");
                    stringBuilder.Append(day);

                    node = new TreeNode() { ParentID = i + 1, NodeID = date.Count + fileList.Count - j, NodeName = stringBuilder.ToString() };
                    nodes.Add(node);

                    //添加三级菜单
                    for (int l = 0; l < lauchTimeList.Count; ++l)
                    {
                        System.Windows.Controls.MenuItem thirdLevelItem = new System.Windows.Controls.MenuItem();
                        node = new TreeNode() { ParentID = date.Count + fileList.Count - j, NodeID = date.Count + logFileList.Count + l + 1, NodeName = lauchTimeList[l].ToString() + "   BodorCut.exe" };
                        nodes.Add(node);
                    }
                }
            }
        }


        public class TreeNode
        {
            public int NodeID { get; set; }
            public int ParentID { get; set; }
            public string NodeName { get; set; }
            public List<TreeNode> ChildNodes { get; set; }
            public TreeNode()
            {
                ChildNodes = new List<TreeNode>();
            }
        }




        //////////////////////////////////////////////////////////////////////////////////////////
        public static List<string> date = new List<string>();
        public static List<string> timeList = new List<string>();
        public static List<string> dateList = new List<string>();
        public static List<string> lauchTimeList = new List<string>();
        public static List<string> logFileList = new List<string>();

        public List<string> GetLogFileList()
        {
            List<string> logFileList = new List<string>();

            try
            {
                DirectoryInfo TheFolder = new DirectoryInfo("D:\\bdData\\logs");

                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    if (NextFile.Name.IndexOf("Normal.txt") != -1)
                    {
                        logFileList.Add(NextFile.Name);
                    }
                }
                return logFileList;
            }
            catch (Exception error)
            {
                return logFileList;
            }
        }

        private List<string> GetDateList()
        {
            for (int i = logFileList.Count - 1; i >= 0; i--)
            {
                string tmp = logFileList[i].Substring(0, 6);
                tmp = tmp.Insert(4, "-");
                date.Add(tmp);
            }
            return date;
        }

        public string GetLogText(string path)
        {
            string logText = string.Empty;

            try
            {
                FileStream fileStream = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fileStream);

                logText = sr.ReadToEnd();
                sr.Close();
                fileStream.Close();

                return logText;
            }
            catch (Exception error)
            {
                return logText;
            }
        }

        private void GetDateAndLanuchTime(List<string> timeList, ref List<string> dateList,
                                    List<string> lauchTimeList, string logText, string pattern)
        {
            timeList.Clear();
            dateList.Clear();
            lauchTimeList.Clear();

            foreach (Match match in Regex.Matches(logText, pattern))
            {
                string tmp = match.Value.Substring(match.Value.IndexOf("(") + 1, match.Value.IndexOf(")") - 1);
                timeList.Add(tmp);
            }

            foreach (var each in timeList)
            {
                dateList.Add(each.Substring(0, each.IndexOf(" ")));
                lauchTimeList.Add(each.Substring(each.IndexOf(" ") + 1));
            }
            dateList = dateList.Distinct().ToList();
        }
    }
}

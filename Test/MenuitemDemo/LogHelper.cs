using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LogHelper
{
    public static class LogDataRefrush
    {
        public static LogMessage GetDetial(string Line)
        {
            LogMessage logMessage = new LogMessage();
            try
            {
                if (Line.IndexOf(" ") != -1)
                {
                    var sArray = Line.Split(' ');
                    logMessage.Time = string.Format("{0} {1} ", sArray[0], sArray[1]);
                    logMessage.MessageType = sArray[2];

                    StringBuilder builder = new StringBuilder(sArray[3]);
                    for (int i = 4; i < sArray.Count(); ++i)
                        builder = builder.Append(sArray[i]);
                    logMessage.Message = builder.ToString();
                }
                else
                {
                    logMessage.MessageType = "普通";
                    logMessage.Message = Line;
                }

                return logMessage;
            }
            catch (Exception error)
            {
                //图形类的信息,类型后直接换行,不存在空格,indexof出错,直接返回
                return logMessage;
            }
        }

        public static int ConvertMessageType(string messageType)
        {
            if (messageType == "普通")
            {
                return (int)LogMessageType.Normal;
            }
            else if (messageType == "进入模块")
            {
                return (int)LogMessageType.EnterModule;
            }
            else if (messageType == "坐标相关")
            {
                return (int)LogMessageType.AboutAxis;
            }
            else
            {
                return (int)LogMessageType.WarningInfo;
            }
        }

        public static List<string> GetLogFileList()
        {
            List<string> logFileList = new List<string>();
            try
            {
                DirectoryInfo TheFolder = new DirectoryInfo(LogFilePath);

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

        public static string GetLogText(string path)
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

        public static void GetDateAndLanuchTime(List<string> timeList, ref List<string> dateList,
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
        public static string LogFilePath { get { return @"C:\Users\29572\Desktop\llll"; } }
    }

    public class LogMessage
    {
        public string Time { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
    }

    public enum LogMessageType
    {
        Normal,
        EnterModule,
        AboutAxis,
        WarningInfo
    }
}

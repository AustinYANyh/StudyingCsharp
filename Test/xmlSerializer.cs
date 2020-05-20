using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System;
using System.Xml.Serialization;

namespace vscode
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 注释掉
            /*
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", null);//版本,编码

            xml.AppendChild(xmlDeclaration);//xml顶部

            XmlElement MyXmlElement = xml.CreateElement("MyXmlElement");
            xml.AppendChild(MyXmlElement);

            XmlElement Name = xml.CreateElement("Name");
            Name.InnerText = "染墨灬若流云";
            MyXmlElement.AppendChild(Name);

            //同一个节点添加多个属性值
            XmlElement Rectangle = xml.CreateElement("Rectangle");
            Rectangle.SetAttribute("Length", "10");
            Rectangle.SetAttribute("Widh", "25");

            MyXmlElement.AppendChild(Rectangle);
            xml.Save("123.xml");

            StreamReader streamReader = new StreamReader("./123.xml");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlDocument));
            var a = xmlSerializer.Deserialize(streamReader);
            */
            #endregion

            List<TestDetial> detial = new List<TestDetial>
            {
                new TestDetial{ID=1,Time=DateTime.Now.ToString()},
                new TestDetial{ID=2,Time=DateTime.Now.ToString()},
                new TestDetial{ID=3,Time=DateTime.Now.ToString()}
            };

            Test test = new Test { Name = "test", TestDetials = detial };

            string xml = XMLHelper.WriteXml<Test>(test, "./123.xml");
            Console.WriteLine(xml);

            Test test1 = XMLHelper.ReadXml<Test>(xml);
            Console.WriteLine(test1.Name + "\n");

            foreach (var item in test1.TestDetials)
            {
                Console.WriteLine(item.ID);
                Console.WriteLine(item.Time + "\n");
            }
        }
    }

    public class Test
    {
        public Test()
        {
            TestDetials = new List<TestDetial>();
        }
        public string Name { get; set; }
        public List<TestDetial> TestDetials { get; set; }
    }

    public class TestDetial
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }
        
        [XmlAttribute("Time")]
        public string Time { get; set; }
    }

    public class XMLHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static string WriteXml<T>(T obj, string path) where T : class
        {
            XmlSerializerNamespaces xn = new XmlSerializerNamespaces();//?
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = UTF8Encoding.UTF8;

            StringWriter stream = new StringWriter();
            using (var writer = XmlWriter.Create(stream, settings))
            {
                xmlSerializer.Serialize(writer, obj, xn);
                return stream.ToString();
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T ReadXml<T>(string xml) where T : class
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer reader = new XmlSerializer(typeof(T));
                return (T)reader.Deserialize(sr);
            }
        }
    }
}

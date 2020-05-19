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
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0","utf-8",null);//版本,编码

            xml.AppendChild(xmlDeclaration);//xml顶部

            XmlElement MyXmlElement  = xml.CreateElement("MyXmlElement");
            xml.AppendChild(MyXmlElement);

            XmlElement Name = xml.CreateElement("Name");
            Name.InnerText="染墨灬若流云";
            MyXmlElement.AppendChild(Name);

            //同一个节点添加多个属性值
            XmlElement Rectangle = xml.CreateElement("Rectangle");
            Rectangle.SetAttribute("Length","10");
            Rectangle.SetAttribute("Widh","25");

            MyXmlElement.AppendChild(Rectangle);
            xml.Save("123.xml");
            
            StreamReader streamReader = new StreamReader("./123.xml");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlDocument));
            var a = xmlSerializer.Deserialize(streamReader);

            Console.WriteLine(streamReader);
        }
    }
}

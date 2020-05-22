using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System;

namespace XMLHelper
{
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
            settings.Encoding = Encoding.UTF8;
            xn.Add("", "");

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
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XmlHelper
{
    public class XMLHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static string WriteXml<T>(T obj) where T : class
        {
            XmlSerializerNamespaces xn = new XmlSerializerNamespaces();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            xn.Add("", "");

            MemoryStream stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, settings))
            {
                xmlSerializer.Serialize(writer, obj, xn);
                return Encoding.UTF8.GetString(stream.ToArray());
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

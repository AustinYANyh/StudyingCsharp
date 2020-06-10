using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System;

namespace vscode
{
    class Program1
    {
        public static void Main()
        {
            Person p1 = new Person();
            p1._Name = "123";
            Person p2 = DeepCopyByXml<Person>(p1);

            p1._Name = "456";
            Console.WriteLine(p1._Name);
            Console.WriteLine(p2._Name);
        }

        public static T DeepCopyByXml<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }

    [Serializable]
    [XmlRoot("Person")]
    public class Person
    {
        public Person()
        {

        }

        public string _Name;
    }
}
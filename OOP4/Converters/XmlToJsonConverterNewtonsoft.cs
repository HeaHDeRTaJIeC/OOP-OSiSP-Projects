using System.IO;
using System.Xml;
using IConverterInterface;

namespace XmlToJsonConverters
{
    public class XmlToJsonConverterNewtonsoft : IConverter
    {
        public string ConvertInput(string source)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter))
            {
                Newtonsoft.Json.JsonConvert.DeserializeXmlNode(source).WriteTo(xmlWriter);
                xmlWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public string ConvertOutput(string source)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(source);
            return Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlDocument);
        }
    }
}

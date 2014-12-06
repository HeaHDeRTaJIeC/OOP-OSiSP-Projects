using System;
using System.IO;

namespace XmlToJsonConverters
{
    class UserConvertationToXml
    {
        public string ConvertToXml(string source)
        {
            tabCount = 0;
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
            var jsonReader = new StringReader(source);

            string textLine = jsonReader.ReadLine();
            while (textLine != null)
            {
                result += ConvertToXmlElement(jsonReader, textLine);
                textLine = jsonReader.ReadLine();
            }

            return result;
        }

        private static int tabCount;

        private string ConvertToXmlElement(StringReader jsonReader, string textLine)
        {
            var xmlNode = new XmlNode(textLine);
            string result = string.Format("{0}<{1} ", new string('\t', tabCount), xmlNode.Name);
            if (xmlNode.Value == "[" || xmlNode.Value == "{")
                result += ConvertToXmlClass(jsonReader, xmlNode.Name);
            else
                result += ConvertToXmlValue(xmlNode.Value, xmlNode.Name);

            return result;
        }

        private string ConvertToXmlClass(StringReader jsonReader, string nodeName)
        {
            string textLine = jsonReader.ReadLine();
            string result = "";
            if (textLine == null) 
                throw new ArgumentException("Invalid JSON");
            if (textLine.Trim().StartsWith("\"atributes\""))
            {
                result += ReadAttributesFromJson(jsonReader);
                textLine = jsonReader.ReadLine();
                if (textLine == null) 
                    throw new ArgumentException("Invalid JSON");
            }
            result += ">\r\n";
            ++tabCount;
            while (textLine.Trim() != "]," && textLine.Trim() != "},")
            {
                result += ConvertToXmlElement(jsonReader, textLine);
                textLine = jsonReader.ReadLine();
                if (textLine == null) 
                    throw new ArgumentException("Invalid JSON");
            }
            --tabCount;
            result += string.Format("{0}</{1}>\r\n", new string('\t', tabCount), nodeName);
            return result;
        }

        private string ConvertToXmlValue(string nodeValue, string nodeName)
        {
            return string.IsNullOrEmpty(nodeValue) ? "/>\r\n" : string.Format(">{0}</{1}>\r\n", nodeValue, nodeName); 
        }

        private string ReadAttributesFromJson(StringReader jsonReader)
        {
            string result = "";
            string textLine = jsonReader.ReadLine();
            if (textLine == null) 
                throw new ArgumentException("Invalid JSON");
            while (textLine.Trim() != "],")
            {
                var xmlNode = new XmlNode(textLine);
                result += string.Format(" {0}=\"{1}\"", xmlNode.Name, xmlNode.Value);
                textLine = jsonReader.ReadLine();
                if (textLine == null) 
                    throw new ArgumentException("Invalid JSON");
            }

            return result;
        }



        private class XmlNode
        {
            public XmlNode(string node)
            {
                string[] nodeParts = node.Split(new[] {"\" : "}, StringSplitOptions.RemoveEmptyEntries);

                Name = nodeParts[0].Trim().Trim('"');
                Value = nodeParts[1].Trim().Trim('"', ',');
            }

            public string Name { get; private set; }
            public string Value { get; private set;}
        }

    }
}

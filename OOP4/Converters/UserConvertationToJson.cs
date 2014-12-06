using System;
using System.IO;
using System.Xml;

namespace XmlToJsonConverters
{
    class UserConvertationToJson
    {
        public string ConvertToJson(string xmlDocument)
        {
            string result = "";
            tabCount = 0;
            var xmlReader = XmlReader.Create(new StringReader(xmlDocument));
            while (xmlReader.Read())
                result += ConvertToJsonElement(xmlReader);

            return result;
        }

        private static int tabCount;

        private string ConvertToJsonElement(XmlReader xmlReader)
        {
            string result = "";
            
            if (xmlReader.NodeType == XmlNodeType.Element && !xmlReader.IsEmptyElement)
                if (xmlReader.Name.StartsWith("Array"))
                    result += ConvertToJsonClass(xmlReader, '[', ']');
                else
                    result += ConvertToJsonClass(xmlReader, '{', '}');

            return result;
        }

        private string ConvertToJsonClass(XmlReader xmlReader, char openChar, char closeChar)
        {
            string result = String.Format("{0}\"{1}\" :", new string('\t', tabCount), xmlReader.Name);
            ++tabCount;
            if (xmlReader.IsEmptyElement)
                return ConvertToJsonEmptyValue(result);

            if (xmlReader.HasAttributes)
            {
                result += string.Format(" {0}\r\n", openChar);
                result += ConvertToJsonAttributes(xmlReader);
                xmlReader.Read();
            }
            else
            {
                xmlReader.Read();
                if (xmlReader.NodeType == XmlNodeType.Text)
                {
                    result += ConvertToJsonText(xmlReader);
                    tabCount--;
                    return result;
                }
                result += string.Format(" {0}\r\n", openChar);   
            }

            do
            {
                result += ConvertToJsonElement(xmlReader);
            } while (xmlReader.Read() && xmlReader.NodeType != XmlNodeType.EndElement);
            --tabCount;
            result += string.Format("{0}{1},\r\n", new string('\t', tabCount), closeChar);

            return result;
        }

        private string ConvertToJsonAttributes(XmlReader xmlReader)
        {
            string result = "";
            if (xmlReader.HasAttributes)
            {
                result = string.Format("{0}\"atributes\" : [\r\n", new string('\t', tabCount));
                xmlReader.MoveToFirstAttribute();
                ++tabCount;
                for (int i = 0; i < xmlReader.AttributeCount; i++)
                {
                    result += new string('\t', tabCount) + 
                        string.Format("\"{0}\" : \"{1}\",\r\n", xmlReader.Name, xmlReader.Value);
                    xmlReader.MoveToNextAttribute();
                }
                --tabCount;
                result += string.Format("{0}],\r\n", new string('\t', tabCount));
                xmlReader.MoveToElement();
            }
            return result;
        }

        private string ConvertToJsonEmptyValue(string result)
        {
            result += " \"\"\r\n";
            tabCount--;
            return result;
        }

        private string ConvertToJsonText(XmlReader xmlReader)
        {
            string result = "";
            result += string.Format(" \"{0}\",\r\n", xmlReader.Value);
            xmlReader.Read();
            return result;
        }
    }
}

using IConverterInterface;

namespace XmlToJsonConverters
{
    public class XmlToJsonConverterUser : IConverter
    {
        public string ConvertInput(string source)
        {
            var converter = new UserConvertationToXml();
            return converter.ConvertToXml(source);
        }

        public string ConvertOutput(string source)
        {
            var converter = new UserConvertationToJson();
            return converter.ConvertToJson(source);
        }
    }
}

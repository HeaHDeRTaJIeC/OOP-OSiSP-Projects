using IConverterInterface;

namespace XmlToJsonConverters
{
    public class XmlToJsonConverterEmpty : IConverter
    {
        public string ConvertInput(string source)
        {
            return source;
        }

        public string ConvertOutput(string source)
        {
            return source;
        }
    }
}

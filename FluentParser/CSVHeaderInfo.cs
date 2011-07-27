using System.Reflection;
using FluentParser.Attributes;

namespace FluentParser
{
    public class CSVHeaderInfo
    {
        public CSVFieldAttribute HeaderAttribute { get; set; }
        public PropertyInfo Property { get; set; }
    }
}

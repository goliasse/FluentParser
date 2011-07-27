using System;

namespace FluentParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CSVFieldAttribute : Attribute
    {
        public bool IsOptional { get; set; }
        public string Header { get; set; }
        public int Position { get; set; }

        public CSVFieldAttribute(string fieldHeader)
            : this(fieldHeader, false)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldHeader">String to look for in the header (ie: "MEMBERID")</param>
        /// <param name="isOptional">is the field optional? (false by default)</param>
        public CSVFieldAttribute(string fieldHeader, bool isOptional)
        {
            this.Header = fieldHeader;
            this.IsOptional = isOptional;
            this.Position = -1;
        }

        public CSVFieldAttribute(int position)
        {
            this.Position = position;
        }
    }
}

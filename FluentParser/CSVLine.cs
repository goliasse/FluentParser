using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FluentParser
{
    public class CSVLine
    {
        public bool IsEOF { get; set; }
        private List<Field> fields;
        private string line;

        public CSVLine(string line, bool isEOF)
        {
            this.line = line;
            this.IsEOF = isEOF;
            if(!isEOF)
                this.fields = CSVParser.ParseCSVLine(line);
        }

        public Field this[int index]
        {
            get
            {
                return GetField(index);
            }
        }

        public Field GetField(int index)
        {
            return fields[index];
        }

        public override string ToString()
        {
            return line;
        }
    }
}

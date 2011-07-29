using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentParser
{
    public class FixedWidthLine
    {
        private string line;
        public bool IsEOF { get; private set; }
        public int LineNumber { get; private set; }

        public Field this[int startIndex, int endIndex]
        {
            get
            {
                if (startIndex > endIndex)
                {
                    throw new System.AccessViolationException();
                }

                return GetField(startIndex, endIndex);
            }
        }

        public FixedWidthLine(string line, bool isEOF)
        {
            this.line = line;
            this.IsEOF = isEOF;
        }

        public Field GetField(int startIndex, int endIndex)
        {
            if (endIndex < startIndex)
                return null;

            int len = endIndex - startIndex;
            return new Field(line.Substring(startIndex, len));
        }

        public override string ToString()
        {
            return line;
        }
    }
}

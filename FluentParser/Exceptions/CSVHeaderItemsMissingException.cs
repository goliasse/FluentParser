using System;
using System.Collections.Generic;

namespace FluentParser.Exceptions
{
    public class CSVHeaderItemsMissingException : Exception
    {
        public List<string> MissingFields { get; set; }
        public CSVHeaderItemsMissingException(List<string> missingFields)
        {
            this.MissingFields = missingFields;
        }
    }
}

using System.Collections.Generic;

namespace FluentParser
{
    public enum HeaderParseSuccess
    {
        Success,
        Failed,
        RequiredFieldsMissing
    };

    public class CSVHeaderParseResult
    {
        public List<string> MissingRequiredFields { get; private set; }
        public List<CSVHeaderInfo> Headers { get; set; }
        public HeaderParseSuccess HeaderParseSuccess { get; private set; }

        public CSVHeaderParseResult(List<CSVHeaderInfo> headers)
        {
            this.MissingRequiredFields = new List<string>();
            this.Headers = headers;

            foreach (var header in headers)
            {
                if (!header.HeaderAttribute.IsOptional && header.HeaderAttribute.Position == -1)
                {
                    MissingRequiredFields.Add(header.HeaderAttribute.Header);
                }
            }

            if (this.MissingRequiredFields.Count == 0)
                this.HeaderParseSuccess = HeaderParseSuccess.Success;
            else
                this.HeaderParseSuccess = HeaderParseSuccess.Failed;
        }
    }
}

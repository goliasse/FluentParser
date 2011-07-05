using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FluentParser
{
    public class CSVFile : IDisposable
    {
        private string _fullpath;
        private StreamReader _filestream;

        public CSVFile(string fullpath)
        {
            _fullpath = fullpath;
        }

        public void Open()
        {
            _filestream = new StreamReader(File.OpenRead(_fullpath));
        }

        public CSVLine ReadLine()
        {
            string line = _filestream.ReadLine();

            if (line == null)
                return new CSVLine(line, isEOF: true);

            return new CSVLine(line, isEOF: false);
        }

        public void Dispose()
        {
            if (_filestream != null)
            {
                _filestream.Dispose();
                _filestream = null;
            }
        }
    }
}

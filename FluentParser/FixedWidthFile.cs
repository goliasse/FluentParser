using System;
using System.IO;

namespace FluentParser
{
    public class FixedWidthFile : IDisposable
    {
        private string _fullpath;
        private TextReader _filestream;

        public string FileName { get; private set; }

        /// <summary>
        /// Open a Fixed Width file for parsing
        /// </summary>
        /// <param name="fullpath"></param>
        public FixedWidthFile(string fullpath)
        {
            this._fullpath = fullpath;
            this.FileName = _fullpath.Substring(_fullpath.LastIndexOf('\\') + 1);
        }

        /// <summary>
        /// Opens the file referenced by the constructor.
        /// Throws all the File.OpenRead exceptions
        /// </summary>
        public void Open()
        {
            _filestream = TextReader.Synchronized(new StreamReader(File.OpenRead(_fullpath)));
        }

        public FixedWidthLine ReadLine()
        {
            string line = _filestream.ReadLine();

            if (line == null)
                return new FixedWidthLine(line, isEOF: true);

            return new FixedWidthLine(line, isEOF: false);
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

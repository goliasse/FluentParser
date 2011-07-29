using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentParser
{
    public class Field
    {
        private string _fieldString;

        public Field(string fieldStr)
        {
            this._fieldString = fieldStr;
        }

        public int ToInt()
        {
            return int.Parse(_fieldString);
        }

        public int? TryToInt()
        {
            int result;
            if (int.TryParse(_fieldString, out result))
                return result;

            return null;
        }

        public Int64 ToInt64()
        {
            return Int64.Parse(_fieldString);
        }

        public Int64? TryToInt64()
        {
            Int64 result;
            if (Int64.TryParse(_fieldString, out result))
                return result;

            return null;
        }

        public decimal ToDecimal()
        {
            return decimal.Parse(_fieldString);
        }

        public decimal? TryToDecimal()
        {
            decimal result;
            if (decimal.TryParse(_fieldString, out result))
            {
                return result;
            }

            return null;
        }

        public float ToFloat()
        {
            return float.Parse(_fieldString);
        }

        public float? TryToFloat()
        {
            float result;
            if (float.TryParse(_fieldString, out result))
            {
                return result;
            }

            return null;
        }

        public DateTime ToDateTime()
        {
            return DateTime.Parse(_fieldString);
        }

        public DateTime TryToDateTime()
        {
            DateTime result;
            if (DateTime.TryParse(_fieldString, out result))
            {
                return result;
            }

            return new DateTime();
        }

        public override string ToString()
        {
            return _fieldString;
        }
    }
}

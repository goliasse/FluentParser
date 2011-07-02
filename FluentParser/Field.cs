using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentParser
{
    public class Field
    {
        private string p;

        public Field(string p)
        {
            this.p = p;
        }

        public int? ToInt()
        {
            int result;
            if (int.TryParse(p, out result))
                return result;

            return null;
        }

        public decimal? ToDecimal()
        {
            decimal result;
            if (decimal.TryParse(p, out result))
            {
                return result;
            }

            return null;
        }

        public float? ToFloat()
        {
            float result;
            if (float.TryParse(p, out result))
            {
                return result;
            }

            return null;
        }

        public DateTime ToDateTime()
        {
            DateTime result;
            if (DateTime.TryParse(p, out result))
            {
                return result;
            }

            return new DateTime();
        }

        public override string ToString()
        {
            return p;
        }
    }
}

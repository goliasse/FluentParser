using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject1
{
    public class ItemEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string UPC { get; set; }
        public DateTime EffectiveDate { get; set; }

        public string Size { get; set; }
    }
}

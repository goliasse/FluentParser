using System;
using System.Collections.Generic;
using System.IO;
using FluentParser;
using FluentParser.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    public class ExampleEntityParseByHeader
    {
        [CSVField("Item Name")]
        public string Name { get; set; }

        [CSVField("Price")]
        public decimal Price { get; set; }

        [CSVField("Brand")]
        public string Brand { get; set; }

        [CSVField("UPC")]
        public string UPC { get; set; }

        [CSVField("Effective Date")]
        public DateTime EffectiveDate { get; set; }

        [CSVField("Size")]
        public string Size { get; set; }
    }


    public class ExampleEntityParseByPosition
    {
        [CSVField(0)]
        public string Name { get; set; }

        [CSVField(1)]
        public decimal Price { get; set; }

        [CSVField(4)]
        public string Brand { get; set; }

        [CSVField(2)]
        public string UPC { get; set; }

        [CSVField(5)]
        public DateTime EffectiveDate { get; set; }

        [CSVField(3)]
        public string Size { get; set; }
    }

    [TestClass]
    public class CSVFileHeaderTests
    {
        private const string filename = "csvtest.csv";
        private const string headerline = "Item Name,Price,UPC,Size,Brand,Effective Date";
        private const string secondline = "Pepsi Max,1.69,0120000011880,20oz,Pepsi,06/18/2011";

        [TestInitialize]
        public void Setup()
        {
            using (var testfile = new StreamWriter(File.OpenWrite(filename)))
            {
                testfile.WriteLine(headerline);
                testfile.WriteLine(secondline);
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            File.Delete(filename);
        }

        [TestMethod]
        public void ReadAndParseFirstLine()
        {
            ExampleEntityParseByHeader entity;

            using (var file = new CSVFile(filename))
            {
                file.Open();
                List<ExampleEntityParseByHeader> entities = CSVParser.ReadAndParseCSV<ExampleEntityParseByHeader>(file.Stream);
                entity = entities[0];
            }

            Assert.AreEqual("Pepsi Max", entity.Name);
            Assert.AreEqual(1.69M, entity.Price);
            Assert.AreEqual("0120000011880", entity.UPC);
            Assert.AreEqual("20oz", entity.Size);
            Assert.AreEqual("Pepsi", entity.Brand);
            Assert.AreEqual(new DateTime(2011, 6, 18), entity.EffectiveDate);
        }
    }
}

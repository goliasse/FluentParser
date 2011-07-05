using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using FluentParser;

namespace TestProject1
{
    [TestClass]
    public class CSVFileTests
    {
        private const string filename = "csvtest.csv";
        private const string firstline = "Pepsi Max,1.69,0120000011880,20oz,Pepsi,06/18/2011";

        [TestInitialize]
        public void Setup()
        {
            using (var testfile = new StreamWriter(File.OpenWrite(filename)))
            {
                testfile.WriteLine(firstline);
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            File.Delete(filename);
        }

        [TestMethod]
        public void OpensFile()
        {
            using (var file = new CSVFile(filename))
            {
                file.Open();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ThrowsFileNotFoundWhenDoesNotExist()
        {
            using (var file = new CSVFile("doesnotexist.txt"))
            {
                try
                {
                    file.Open();
                }
                catch (FileNotFoundException e)
                {
                    Assert.IsTrue(true);
                    return;
                }

                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void ReadEmptyLineReturnsNull()
        {
            File.Delete(filename);

            var testfile = new StreamWriter(File.OpenWrite(filename));
            testfile.WriteLine("");
            testfile.Close();

            using (var file = new CSVFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                Assert.AreEqual(string.Empty, line.ToString());
            }
        }

        [TestMethod]
        public void ReadSingleLine()
        {
            using (var file = new CSVFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                Assert.AreEqual(firstline, line.ToString());
            }
        }

        [TestMethod]
        public void ReadUntilEOL()
        {
            using (var file = new CSVFile(filename))
            {
                file.Open();
                CSVLine line;
                do
                {
                    line = file.ReadLine();
                } while (!line.IsEOF);

                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void ReadAndParseFirstLine()
        {
            ItemEntity entity = new ItemEntity();
            using (var file = new CSVFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                entity.Name = line.GetField(0).ToString().Trim();
                entity.Price = line.GetField(1).TryToDecimal() ?? 0;
                entity.UPC = line.GetField(2).ToString();
                entity.Size = line.GetField(3).ToString().Trim();
                entity.Brand = line.GetField(4).ToString().Trim();
                entity.EffectiveDate = line.GetField(5).TryToDateTime();
            }

            Assert.AreEqual("Pepsi Max", entity.Name);
            Assert.AreEqual(1.69M, entity.Price);
            Assert.AreEqual("0120000011880", entity.UPC);
            Assert.AreEqual("20oz", entity.Size);
            Assert.AreEqual("Pepsi", entity.Brand);
            Assert.AreEqual(new DateTime(2011, 6, 18), entity.EffectiveDate);
        }

        [TestMethod]
        public void AlternateFieldAccessMethod()
        {
            ItemEntity entity = new ItemEntity();
            using (var file = new CSVFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                entity.Name = line[0].ToString().Trim();
                entity.Price = line[1].TryToDecimal() ?? 0;
                entity.UPC = line[2].ToString();
                entity.Size = line[3].ToString().Trim();
                entity.Brand = line[4].ToString().Trim();
                entity.EffectiveDate = line[5].TryToDateTime();
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

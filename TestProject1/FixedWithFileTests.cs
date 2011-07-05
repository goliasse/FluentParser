using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentParser;
using System.IO;

namespace TestProject1
{
    [TestClass]
    public class FixedWidthFileTests
    {
        private const string filename = "testfixedwith.txt";
        private const string firstLine = "Pepsi Max          00001.6900012000001188020oz      Pepsi               06/18/2011";

        [TestInitialize]
        public void SetUp()
        {
            var testfile = new StreamWriter(File.OpenWrite(filename));
            testfile.WriteLine(firstLine);
            testfile.Close();
        }

        [TestCleanup]
        public void TearDown()
        {
            File.Delete(filename);
        }

        [TestMethod]
        public void OpensFile()
        {
            using (var file = new FixedWidthFile(filename))
            {
                file.Open();
            }

            Assert.IsTrue(true);

            File.Delete(filename);
        }

        [TestMethod]
        public void ThrowsFileNotFoundWhenDoesNotExist()
        {
            using (var file = new FixedWidthFile("doesnotexist.txt"))
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

            using (var file = new FixedWidthFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                Assert.AreEqual(string.Empty, line.ToString());
            }
        }

        [TestMethod]
        public void ReadSingleLine()
        {
            using (var file = new FixedWidthFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                Assert.AreEqual(firstLine, line.ToString());
            }
        }

        [TestMethod]
        public void ReadUntilEOL()
        {
            using (var file = new FixedWidthFile(filename))
            {
                file.Open();
                FixedWidthLine line;
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
            using (var file = new FixedWidthFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                entity.Name = line.GetField(0, 19).ToString().Trim();
                entity.Price = line.GetField(20, 29).TryToDecimal() ?? 0;
                entity.UPC = line.GetField(29, 42).ToString();
                entity.Size = line.GetField(42, 52).ToString().Trim();
                entity.Brand = line.GetField(52, 72).ToString().Trim();
                entity.EffectiveDate = line.GetField(73, 82).TryToDateTime();
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
            using (var file = new FixedWidthFile(filename))
            {
                file.Open();
                var line = file.ReadLine();
                entity.Name = line[0, 19].ToString().Trim();
                entity.Price = line[20, 29].TryToDecimal() ?? 0;
                entity.UPC = line[29, 42].ToString();
                entity.Size = line[42, 52].ToString().Trim();
                entity.Brand = line[52, 72].ToString().Trim();
                entity.EffectiveDate = line[73, 82].TryToDateTime();
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
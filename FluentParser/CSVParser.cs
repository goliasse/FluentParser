//***********************************************************************************************
// Original Code was taken from: http://www.codeproject.com/KB/recipes/Basic_CSV_Parser_Function.aspx
// Modified by Jim Wallace.
//***********************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using FluentParser.Attributes;
using FluentParser.Exceptions;
using System;

namespace FluentParser
{
    public class CSVParser
    {
        public static List<Field> ParseCSVLine(string strInputString)
        {
            int intCounter = 0, intLenght;
            StringBuilder strElem = new StringBuilder();
            var alParsedCsv = new List<Field>();
            intLenght = strInputString.Length;
            strElem = strElem.Append("");
            int intCurrState = 0;
            int[][] aActionDecider = new int[9][];
            //Build the state array
            aActionDecider[0] = new int[4] { 2, 0, 1, 5 };
            aActionDecider[1] = new int[4] { 6, 0, 1, 5 };
            aActionDecider[2] = new int[4] { 4, 3, 3, 6 };
            aActionDecider[3] = new int[4] { 4, 3, 3, 6 };
            aActionDecider[4] = new int[4] { 2, 8, 6, 7 };
            aActionDecider[5] = new int[4] { 5, 5, 5, 5 };
            aActionDecider[6] = new int[4] { 6, 6, 6, 6 };
            aActionDecider[7] = new int[4] { 5, 5, 5, 5 };
            aActionDecider[8] = new int[4] { 0, 0, 0, 0 };
            for (intCounter = 0; intCounter < intLenght; intCounter++)
            {
                intCurrState = aActionDecider[intCurrState]
                                          [GetInputID(strInputString[intCounter])];
                //take the necessary action depending upon the state 
                PerformAction(ref intCurrState, strInputString[intCounter],
                             ref strElem, ref alParsedCsv);
            }
            //End of line reached, hence input ID is 3
            intCurrState = aActionDecider[intCurrState][3];
            PerformAction(ref intCurrState, '\0', ref strElem, ref alParsedCsv);

            return alParsedCsv;
        }

        private static int GetInputID(char chrInput)
        {
            if (chrInput == '"')
            {
                return 0;
            }
            else if (chrInput == ',')
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        private static void PerformAction(ref int intCurrState, char chrInputChar,
                            ref StringBuilder strElem, ref List<Field> alParsedCsv)
        {
            string strTemp = null;
            switch (intCurrState)
            {
                case 0:
                    //Separate out value to array list
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(new Field(strTemp));
                    strElem = new StringBuilder();
                    break;
                case 1:
                case 3:
                case 4:
                    //accumulate the character
                    strElem.Append(chrInputChar);
                    break;
                case 5:
                    //End of line reached. Separate out value to array list
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(new Field(strTemp));
                    break;
                case 6:
                    //Erroneous input. Reject line.
                    alParsedCsv.Clear();
                    break;
                case 7:
                    //wipe ending " and Separate out value to array list
                    strElem.Remove(strElem.Length - 1, 1);
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(new Field(strTemp));
                    strElem = new StringBuilder();
                    intCurrState = 5;
                    break;
                case 8:
                    //wipe ending " and Separate out value to array list
                    strElem.Remove(strElem.Length - 1, 1);
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(new Field(strTemp));
                    strElem = new StringBuilder();
                    //goto state 0
                    intCurrState = 0;
                    break;
            }
        }

        public static List<T> ReadAndParseCSV<T>(StreamReader s) where T : new()
        {
            var items = new List<T>();
            TextReader reader = s; 

            string line = string.Empty;
            line = reader.ReadLine();

            var result = VerifyHeader<T>(line);

            if (result.HeaderParseSuccess == HeaderParseSuccess.Failed)
            {
                throw new CSVHeaderItemsMissingException(result.MissingRequiredFields);
            }

            int lineNumber = 1;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (!string.IsNullOrEmpty(line))
                {
                    var rawValues = CSVParser.ParseCSVLine(line);
                    T newInstanceOfEntity = new T();

                    foreach (var header in result.Headers)
                    {
                        if (header.HeaderAttribute.Position != -1)
                        {
                            var value = rawValues[header.HeaderAttribute.Position];

                            if (!string.IsNullOrEmpty(value.ToString()))
                            {
                                header.Property.SetValue(newInstanceOfEntity,
                                    Convert.ChangeType(value.ToString(), header.Property.PropertyType), null);
                            }
                        }
                    }

                    items.Add(newInstanceOfEntity);
                }
            }

            return items;
        }

        private static CSVHeaderParseResult VerifyHeader<T>(string line)
        {
            // parse the header line and locate the positions
            // of each of the fields.
            var headerValues = CSVParser.ParseCSVLine(line);
            bool headerExists = false;

            // Reflect over Trade object and find all the CSV attributes
            var props = typeof(T).GetProperties();

            int blankHeaderFieldCount = 0;
            int nonBlankHeaderFieldCount = 0;

            List<CSVHeaderInfo> header_info = new List<CSVHeaderInfo>();

            foreach (var prop in props)
            {
                var attributes = prop.GetCustomAttributes(typeof(CSVFieldAttribute), true);
                foreach (CSVFieldAttribute attribute in attributes)
                {
                    var csvFieldAttribute = attributes[0] as CSVFieldAttribute;

                    CSVHeaderInfo info = new CSVHeaderInfo();
                    info.HeaderAttribute = csvFieldAttribute;
                    info.Property = prop;

                    if (!string.IsNullOrEmpty(csvFieldAttribute.Header))
                    {
                        nonBlankHeaderFieldCount++;

                        for (int i = 0; i < headerValues.Count; i++)
                        {
                            if (headerValues[i].ToString().ToUpperInvariant() == csvFieldAttribute.Header.ToUpperInvariant())
                            {
                                csvFieldAttribute.Position = i;
                                headerExists = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        blankHeaderFieldCount++;
                    }

                    header_info.Add(info);
                }
            }

            if ((blankHeaderFieldCount != nonBlankHeaderFieldCount) && !headerExists)
            {
                throw new CSVNoHeaderException();
            }

            return new CSVHeaderParseResult(header_info);
        }
    }
}

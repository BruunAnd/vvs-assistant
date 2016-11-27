using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO.Compression;
using System.Linq;
using System.Text;
using VVSAssistant.Database;
using VVSAssistant.Models;

namespace VVSAssistant.Functions
{
    public static partial class DataUtil
    {

        public static class SalesCatalogue
        {
            public static bool Import(string src)
            {
                if (!ValidateFormat(src))
                {
                    return false;
                }
                var reader = new StreamReader(File.OpenRead(src));
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    DataParser(line);
                }
                reader.Close();
                return true;
            }

            public static bool ValidateFormat(string src)
            {
                var reader = new StreamReader(File.OpenRead(src));
                var values = ProcessLine(reader.ReadLine());
                return values.Length == 4;
            }

            private static string[] ProcessLine(string line)
            {
                var values = Array.ConvertAll(line.Split(';'), p => p.Trim('\"'));
                return values;
            }

            private static void DataParser(string line)
            {
                var values = ProcessLine(line);
            }
        }


    }
}

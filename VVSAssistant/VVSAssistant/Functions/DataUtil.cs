using System;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.IO.Compression;
using System.Linq;

namespace VVSAssistant.Functions
{
    public static class DataUtil
    {
        public static class Database
        {
            public static string Name()
            {
                return new AppSettingsReader().GetValue("dbFileName", typeof(string)).ToString();
            }

            public static bool Exists()
            {
                return Location() != null;
            }

            private static string Location()
            {
                var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Directory.GetFiles(assemblyLocation, Name()).FirstOrDefault();
            }

            public static bool Export(string targetPath)
            {
                // Path is invalid
                if (!Exists()) return false;
                if(File.Exists(targetPath)) File.Delete(targetPath);
                using (ZipArchive zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(Location(), Name());
                }

                return true;
            }

            public static bool Import(string src)
            {
                var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (var archive = ZipFile.OpenRead(src))
                {
                    if (archive.Entries.FirstOrDefault(x => x.Name == Name()) == null)
                    {
                        return false;
                    }   
                }
                if (Exists()) File.Delete(Location());
                ZipFile.ExtractToDirectory(src, assemblyLocation);
                return true;
            }
        }

        public static class VVSCatalogue
        {
            public static bool Import(string src)
            {
                if (ValidateFormat(src) == false) return false;

                var reader = new StreamReader(File.OpenRead(src));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    DataParser(line);
                }

                return true;
            }

            //private enum EntityProperty
            //{
            //    id = 0,
            //    name = 2,
            //    quantity = 5,

            //}
            
            private static void DataParser(string line)
            {

                var values = line.Split(',');
                
            }

            private static void AddProduct(string values)
            {

            }

            private static void AddMaterials(string values)
            {

            }

            public static bool ValidateFormat(string src)
            {
                var reader = new StreamReader(File.OpenRead(src));
                var line = reader.ReadLine();
                if (line != null)
                {
                    var values = line.Split(',');
                    if (values.Length != 9)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
        }
    }
}

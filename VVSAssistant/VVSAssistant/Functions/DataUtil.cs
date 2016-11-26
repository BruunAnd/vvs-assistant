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
using EntityFramework.BulkInsert.Extensions;
using VVSAssistant.Database;
using VVSAssistant.Models;

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
                if (!ValidateFormat(src))
                {
                    return false;
                }
                var reader = new StreamReader(File.OpenRead(src));
                var materials = new List<Material>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    DataParser(line, materials);
                }
                reader.Close();
                UpdateDatabase(materials);
                return true;
            }

            private static void UpdateDatabase(List<Material> materials)
            {
                var dcontext = new AssistantContext();
                // We don't want to query the database for each new material, so we create a local copy of the list
                var existingMaterials = dcontext.Materials.ToList();
                // We want to bulk insert, so we make a list of new materials to be added
                var newMaterials = new List<Material>();
                foreach (var mat in materials)
                {
                    var entity = existingMaterials.FirstOrDefault(x => x.VvsNumber != null && x.VvsNumber.Equals(mat.VvsNumber));
                    if (entity == null)
                    {
                        newMaterials.Add(mat);
                    }
                    else
                    {
                        entity.Name = mat.Name;
                        entity.UnitSalesPrice = mat.UnitSalesPrice;
                        entity.SpecificationsType = mat.SpecificationsType;
                    }
                }

                // Make bulk insert
                dcontext.BulkInsert(newMaterials);
                dcontext.SaveChanges();
                dcontext.Dispose();
            }

            private enum EntityProperty
            {
                Id = 0,
                SpecType = 1,
                Name = 2,
                ListPrice = 5,

            }
            

            private static void DataParser(string line, List<Material> materials)
            {
                var values = ProcessLine(line);
                materials.Add(new Material
                {
                    VvsNumber = values[(int)EntityProperty.Id],
                    SpecificationsType = values[(int)EntityProperty.SpecType],
                    Name = values[(int)EntityProperty.Name],
                    UnitSalesPrice = double.Parse(values[(int)EntityProperty.ListPrice])
                });
            }

            private static string[] ProcessLine(string line)
            {
                var values = line.Split(',');

                int j = 0;
                for (var i = 0; i < values.Length; i++)
                {
                    if (values[i].StartsWith("\"") && !values[i].EndsWith("\""))
                    {
                        var sb = new StringBuilder();
                        while (!values[i].EndsWith("\""))
                        {
                            sb.Append(values[i++]);
                        }

                        values[j] = sb.ToString().Trim('\"');
                    }
                    else
                    {
                        values[j] = values[i].Trim('\"');
                    }
                    j++;
                }

                return values.ToArray();
            }
            
            public static bool ValidateFormat(string src)
            {
                var reader = new StreamReader(File.OpenRead(src));
                var values = ProcessLine(reader.ReadLine());
                return values.Length == 8;
            }
        }
    }
}

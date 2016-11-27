using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO.Compression;
using System.Linq;
using System.Text;
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

        public static class VvsCatalogue
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

            private static void UpdateDatabase(IEnumerable<Material> materials)
            {
                var context = new AssistantContext();

                var existingMaterials = context.Materials.ToList();
                
                try
                {
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var count = 0;
                    foreach (var mat in materials)
                    {
                        ++count;
                        var entity = existingMaterials.FirstOrDefault(x => x.VvsNumber != null && x.VvsNumber.Equals(mat.VvsNumber));

                        if(entity == null)
                        {
                            context.Materials.Add(mat);
                        }
                        else
                        {
                            if (!entity.Name.Equals(mat.Name) || !entity.UnitSalesPrice.Equals(mat.UnitSalesPrice) || !entity.SpecificationsType.Equals(mat.SpecificationsType))
                            {
                                context.Entry(entity).Property(x => x.Name).CurrentValue = mat.Name;
                                context.Entry(entity).Property(x => x.UnitSalesPrice).CurrentValue = mat.UnitSalesPrice;
                                context.Entry(entity).Property(x => x.SpecificationsType).CurrentValue = mat.SpecificationsType;
                            }
                        }

                        if (count % 1000 != 0) continue;
                        context.SaveChanges();
                        context.Dispose();
                        context = new AssistantContext();
                        context.Configuration.AutoDetectChangesEnabled = false;
                        Console.WriteLine(count);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    context.Dispose();
                }
            }

            private enum EntityProperty
            {
                Id = 0,
                SpecType = 1,
                Name = 2,
                ListPrice = 5,

            }
            

            private static void DataParser(string line, ICollection<Material> materials)
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

                var j = 0;
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

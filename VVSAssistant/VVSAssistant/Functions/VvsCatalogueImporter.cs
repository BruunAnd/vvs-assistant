using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VVSAssistant.Database;
using VVSAssistant.Models;

namespace VVSAssistant.Functions
{
    public static partial class DataUtil
    {
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

            public static bool ValidateFormat(string src)
            {
                var reader = new StreamReader(File.OpenRead(src));
                var values = ProcessLine(reader.ReadLine());
                // The VVS catalogue is always of length 8, so perform quick validation.
                return values.Length == 8;
            }

            private static void UpdateDatabase(IEnumerable<Material> materials)
            {

                var context = new AssistantContext();
                // Get a list of materials already in the database.
                var existingMaterials = context.Materials.ToList();
                
                try
                {
                    // Disable autodetectchanges to avoid validation of every line.
                    context.Configuration.AutoDetectChangesEnabled = false;
                    
                    var count = 0;
                    foreach (var mat in materials)
                    {
                        ++count;
                        // Check if an entity in the database has a matching VVS number with an entity in the parsed list, otherwise set to null.
                        var entity = existingMaterials.FirstOrDefault(x => x.VvsNumber != null && x.VvsNumber.Equals(mat.VvsNumber));

                        // If no entity matches the VVS number add it to the databse.
                        if(entity == null)
                        {
                            context.Materials.Add(mat);
                        }
                        else
                        {
                            // Entity with matching VVS number exists in the database, so check if values need to be updated.
                            if (!entity.Name.Equals(mat.Name) || !entity.UnitCostPrice.Equals(mat.UnitSalesPrice) || !entity.SpecificationsType.Equals(mat.SpecificationsType))
                            {
                                // Update the values for the entity.
                                context.Entry(entity).Property(x => x.Name).CurrentValue = mat.Name;
                                context.Entry(entity).Property(x => x.UnitCostPrice).CurrentValue = mat.UnitCostPrice;
                                context.Entry(entity).Property(x => x.SpecificationsType).CurrentValue = mat.SpecificationsType;
                            }
                        }

                        // Save and dispose the database assistant at every 1000th element to speed up the process.
                        if (count % 1000 != 0) continue;
                        context.SaveChanges();
                        context.Dispose();
                        context = new AssistantContext();
                        context.Configuration.AutoDetectChangesEnabled = false;
                    }
                }
                finally
                {
                    // Save changes and dispose.
                    context.SaveChanges();
                    context.Dispose();
                }
            }

            private enum EntityProperty
            {
                Id = 0,
                SpecType = 1,
                Name = 2,
                ListPrice = 6,

            }
            
            private static void DataParser(string line, ICollection<Material> materials)
            {
                // Process and retrieve values from the read line.
                var values = ProcessLine(line);

                // Add a new material to the list of materials.
                materials.Add(new Material
                {
                    VvsNumber = values[(int)EntityProperty.Id],
                    SpecificationsType = values[(int)EntityProperty.SpecType],
                    Name = values[(int)EntityProperty.Name],
                    UnitCostPrice = double.Parse(values[(int)EntityProperty.ListPrice])
                });
            }

            private static string[] ProcessLine(string line)
            {
                // Split read line in sections separated by ','
                var values = line.Split(',');

                var j = 0;
                for (var i = 0; i < values.Length; i++)
                {
                    // If a section of the read line starts with '"' and does not end with '"' join with sections until a closing '"' is met.
                    if (values[i].StartsWith("\"") && !values[i].EndsWith("\""))
                    {
                        var sb = new StringBuilder();
                        while (!values[i].EndsWith("\""))
                        {
                            sb.Append(values[i++]);
                        }

                        // Remove leading and trailing '"' from the section.
                        values[j] = sb.ToString().Trim('\"');
                    }
                    else
                    {
                        // Remove leading and trailing '"' from the section.
                        values[j] = values[i].Trim('\"');
                    }
                    j++;
                }

                return values;
            }
        }
    }
}

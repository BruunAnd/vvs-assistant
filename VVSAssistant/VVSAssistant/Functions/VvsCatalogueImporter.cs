using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.BulkInsert.SqlServerCe;
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
                var materialReferences = new List<MaterialReference>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    DataParser(line, materialReferences);
                }
                reader.Close();
                UpdateDatabase(materialReferences);
                return true;
            }

            public static bool ValidateFormat(string src)
            {
                var reader = new StreamReader(File.OpenRead(src));
                var values = ProcessLine(reader.ReadLine());
                // The VVS catalogue is always of length 8, so perform quick validation.
                return values.Length == 8;
            }

            private static void UpdateDatabase(IEnumerable<MaterialReference> materialReferences)
            {
                //var connection = new AssistantContext().Database.Connection;
                //connection.Open();
                //var bulk = new BulkOperation(connection) { DestinationTableName = "Materials" };
                //bulk.BulkInsert(materials);
                //using (var ctx = new AssistantContext())
                //{
                //    using (var bcp = new SqlCeBulkCopy(ctx.Database.Connection.ConnectionString))
                //    {
                //        bcp.WriteToServer(materials);

                //        foreach (var k in ctx.Materials)
                //        {
                //            Console.WriteLine(k.Name);
                //        }
                //    }
                //}

                //var context = new AssistantContext();
                //EntityFramework.BulkInsert.ProviderFactory.Register<SqlCeBulkInsertProvider>("System.Data.SqlServerCe.SqlCeConnection");
                //context.BulkInsert(materials);
                //context.Dispose();
                //context = new AssistantContext();
                //foreach (var k in context.Materials)
                //{
                //    Console.WriteLine(k.Name);
                //}



                //try
                //{
                //    // Empty all rows in the database.
                //    context.Database.ExecuteSqlCommand("DELETE FROM Materials");
                //    context.Configuration.AutoDetectChangesEnabled = false;

                //    var count = 0;
                //    foreach (var mat in materials)
                //    {
                //        ++count;
                //        context.Materials.Add(mat);
                //        if (count % 1000 != 0) continue;
                //        context.SaveChanges();
                //        context.Dispose();
                //        context = new AssistantContext();
                //        context.Configuration.AutoDetectChangesEnabled = false;
                //    }

                //    context.SaveChanges();
                //}
                //finally
                //{
                //    context.Dispose();
                //}


            }

            private enum EntityProperty
            {
                Id = 0,
                SpecType = 1,
                Name = 2,
                ListPrice = 6,

            }
            
            private static void DataParser(string line, ICollection<MaterialReference> materialReferences)
            {
                // Process and retrieve values from the read line.
                var values = ProcessLine(line);

                // Add a new material to the list of materials.
                materialReferences.Add(new MaterialReference()
                {
                    VvsNumber = values[(int)EntityProperty.Id],
                    SpecificationsType = values[(int)EntityProperty.SpecType],
                    Name = values[(int)EntityProperty.Name],
                    CostPrice = double.Parse(values[(int)EntityProperty.ListPrice])
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

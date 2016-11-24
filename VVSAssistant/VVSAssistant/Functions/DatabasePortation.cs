using System;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.IO.Compression;
using System.Linq;

namespace VVSAssistant.Functions
{
    public static class DatabasePortation
    {
        public static string Name()
        {
            return new AppSettingsReader().GetValue("dbFileName", typeof(string)).ToString();
        }

        public static bool Exists()
        {
            return DatabasePortation.Location() != null;
        }

        public static string Location()
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Directory.GetFiles(assemblyLocation, DatabasePortation.Name()).FirstOrDefault();
        }

        public static bool Export(string targetPath)
        {
            // Path is invalid
            if (!DatabasePortation.Exists()) return false;
            if(File.Exists(targetPath)) File.Delete(targetPath);
            using (ZipArchive zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(DatabasePortation.Location(), DatabasePortation.Name());
            }

            return true;
        }
        

        public static bool Import(string srcPath)
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (var archive = ZipFile.OpenRead(srcPath))
            {
                if (archive.Entries.FirstOrDefault(x => x.Name == DatabasePortation.Name()) == null)
                {
                    return false;
                }   
            }
            if (DatabasePortation.Exists()) File.Delete(DatabasePortation.Location());
            ZipFile.ExtractToDirectory(srcPath, assemblyLocation);
            return true;
        }
    }
}

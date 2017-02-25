using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Functions
{
    public static class ApplicationDatabase
    {
        private static string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string Name =>
            new AppSettingsReader().GetValue("dbFileName", typeof(string)).ToString();

        public static bool Exists => Location != null;

        private static string Location =>
            Directory.GetFiles(assemblyLocation, Name).FirstOrDefault();

        public static bool Export(string targetPath)
        {
            // Path is invalid
            if (!Exists) return false;
            if (File.Exists(targetPath)) File.Delete(targetPath);
            using (var zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(Location, Name);
            }

            return true;
        }

        public static bool Import(string src)
        {
            using (var archive = ZipFile.OpenRead(src))
            {
                if (archive.Entries.FirstOrDefault(x => x.Name == Name) == null)
                {
                    return false;
                }
            }
            if (Exists) File.Delete(Location);
            ZipFile.ExtractToDirectory(src, assemblyLocation);
            return true;
        }
    }
}

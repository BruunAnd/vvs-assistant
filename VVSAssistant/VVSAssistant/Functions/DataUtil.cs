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
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

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
                return Directory.GetFiles(AssemblyDirectory, Name()).FirstOrDefault();
            }

            public static bool Export(string targetPath)
            {
                // Path is invalid
                if (!Exists()) return false;
                if (File.Exists(targetPath)) File.Delete(targetPath);
                using (var zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
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
    }
}

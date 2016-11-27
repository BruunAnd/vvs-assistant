using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace VVSAssistant.Functions
{
    public static partial class DataUtil
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
                if (File.Exists(targetPath)) File.Delete(targetPath);
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
    }
}
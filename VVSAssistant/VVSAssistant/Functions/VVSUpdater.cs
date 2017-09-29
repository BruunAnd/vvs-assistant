using Squirrel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VVSAssistant.Database;

namespace VVSAssistant.Functions
{
    /// <summary>
    /// Update methods called from App.xaml.cs
    /// </summary>
    class VVSUpdater
    {
        private bool _updatesAvailable = false;
        public bool UpdatesAvailable { get { return _updatesAvailable; } set { _updatesAvailable = value; } }

        private string _serverVersion = "0.0.0";
        public string ServerVersion { get { return _serverVersion; } set { _serverVersion = value; } }

        public string CurrentVersion { get { return GetCurrentVersion(); } set { SetCurrentVersion(value); } }

        private string _localPath { get { return Directory.GetCurrentDirectory(); }
        }

        string _serverPath, _serverUN, _serverPW;

        public VVSUpdater()
        {
            _serverPath = System.Configuration.ConfigurationManager.AppSettings["ServerPath"];
            _serverUN = System.Configuration.ConfigurationManager.AppSettings["ServerUN"];
            _serverPW = System.Configuration.ConfigurationManager.AppSettings["ServerPW"];
        }

        private string GetCurrentVersion()
        {
            string versionFilePath = _localPath + "\\Version.txt";
            if (File.Exists(versionFilePath))
                return File.ReadAllText(versionFilePath);
            else
                return "0.0.0";
 
        }

        /// <summary>
        /// Creates a new Version.txt file with the current version. Must be of format "x.y.z"
        /// </summary>
        private void SetCurrentVersion(string version)
        {
            string versionFilePath = _localPath + "\\Version.txt";
            if (File.Exists(versionFilePath))
            {
                File.Delete(versionFilePath);
                File.WriteAllText(versionFilePath, version);
            }
            else
            {
                File.WriteAllText(versionFilePath, version);
            }
        }

        /// <summary>
        /// Updates the application with update files from the server if a newer version is available.
        /// </summary>
        public void UpdateApplication()
        {
            CheckForUpdates();
            if (UpdatesAvailable)
            {
                DownloadAndUpdateFromServer();
                CurrentVersion = ServerVersion;
            }
        }

        /// <summary>
        /// Save the DB in a folder in the parent dir so a newer version can pick it up
        /// </summary>
        public void SaveDatabase(object src, EventArgs e)
        {
            string parentStoragePath = Directory.GetParent(_localPath).FullName + "\\data";
            if (File.Exists(_localPath + "\\Database.sdf"))
            {
                if (Directory.Exists(parentStoragePath)){
                    File.Delete(parentStoragePath + "\\Database.sdf");
                    File.Copy(_localPath + "\\Database.sdf", parentStoragePath + "\\Database.sdf");
                }
                else {
                    Directory.CreateDirectory(parentStoragePath);
                    File.Copy(_localPath + "\\Database.sdf", parentStoragePath + "\\Database.sdf");
                }
                
            }
        }

        /// <summary>
        /// Replaces the current DB with the one in the parent dir if it exists. 
        /// Deletes the old DB after replacing.
        /// </summary>
        public void LoadExistingDatabase()
        {
            string parentStoragePath = Directory.GetParent(_localPath).FullName + "\\data";
            if (Directory.Exists(parentStoragePath))
            {
                if (File.Exists(parentStoragePath + "\\Database.sdf"))
                {
                    File.Replace(parentStoragePath + "\\Database.sdf", _localPath + "\\Database.sdf", _localPath + "\\DatabaseBackup.sdf");
                }
                Directory.Delete(parentStoragePath, true);
            }
        }

        /// <summary>
        /// Downloads all update files from server and applies updates for next startup. 
        /// WARNING: Takes a long time.
        /// </summary>
        public async void DownloadAndUpdateFromServer()
        {
            string localTempFolderPath = _localPath + "\\tempUpdateFiles";
            Directory.CreateDirectory(localTempFolderPath);

            SaveDatabase(new object(), new EventArgs());

            List<string> filenames = GetServerDirectoryListing();

            using (WebClient ftpClient = new WebClient())
            {
                ftpClient.Credentials = new NetworkCredential(_serverUN, _serverPW);

                //Download all files in "/server/.../VVSUpdateFiles" and put them in temp folder
                for (int i = 0; i <= filenames.Count - 1; i++)
                {
                    if (filenames[i].Contains(".") && filenames[i].Any(c => c != '.'))
                    {
                        string serverFilePath = _serverPath + filenames[i].ToString();
                        string localFilePath = localTempFolderPath + "\\" + filenames[i].ToString();
                        ftpClient.DownloadFile(serverFilePath, localFilePath);
                    }
                }
            }
            //Let Squirrel.Windows take over from here
            using (var mgr = new UpdateManager(localTempFolderPath))
            {
                await mgr.UpdateApp();
            }
            Directory.Delete(localTempFolderPath, true);
        }
        /// <summary>
        /// Check all filenames on server, see if they contain a version number higher than current.
        /// NuGet update packages should contain the version number, which allows us to do this.
        /// Server should only have files related to the newest update, as the method will cancel
        /// on the first occurrence of the version number format.
        /// </summary>
        public void CheckForUpdates()
        {
            List<string> filenames = GetServerDirectoryListing();
            Match match;

            foreach (string filename in filenames)
            {
                match = Regex.Match(filename, "[0-9]+.[0-9]+.[0-9]");
                if (match.Success)
                {
                    ServerVersion = match.Value;
                    UpdatesAvailable = CompareVersions(CurrentVersion, ServerVersion);
                    return;
                }
            }
        }

        /// <summary>
        /// Return list of all filenames and foldernames in the serverPath
        /// </summary>
        private List<string> GetServerDirectoryListing()
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(_serverPath);
            List<string> filenames = new List<string>();

            ftpRequest.Credentials = new NetworkCredential(_serverUN, _serverPW);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            string line = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                filenames.Add(line);  
                line = streamReader.ReadLine();
            }
            streamReader.Close();
            return filenames;
        }

        /// <summary>
        /// Returns true if server version is larger than current version. Must be of format "x.y.z"
        /// </summary>
        public bool CompareVersions(string current, string server)
        {
            string[] currentVer = current.Split('.');
            string[] serverVer = server.Split('.');
            for (int i = 0; i < 3; i++)
            {
                int cur = Convert.ToInt32(currentVer[i]);
                int ser = Convert.ToInt32(serverVer[i]);
                if (ser > cur) return true;
            }
            return false;
        }

        /// <summary>
        /// Destroys the local temp update files if they are still there.
        /// Should be subscribed to event raised when app closes.
        /// </summary>
        public void DeletePartiallyDownloadedUpdateFiles()
        {
            if (Directory.Exists(_localPath + "\\tempUpdateFiles"))
            {
                Directory.Delete(_localPath + "\\tempUpdateFiles", true);
            }
        }
    }
}

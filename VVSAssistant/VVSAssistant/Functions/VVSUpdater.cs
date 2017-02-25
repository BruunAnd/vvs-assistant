using Squirrel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        string _serverPath, _localPath, _serverUN, _serverPW;

        public VVSUpdater()
        {
            _serverPath = "ftp://etilmelding.com/public_ftp/VVS/VVSAssistantUpdates/";
            _localPath = Directory.GetCurrentDirectory();
            _serverUN = "ownerofthis";
            _serverPW = "BramsHansen2";
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
        /// After temp update folder has been added, save the database in there.
        /// </summary>
        private void SaveDatabase()
        {
            if (Directory.Exists(_localPath + "\\tempUpdateFiles\\"))
            {
                File.Copy(_localPath + "\\Database.sdf", _localPath + "\\tempUpdateFiles\\Database.sdf");
            }
        }

        public void ReloadDatabase()
        {
            File.Delete(_localPath + "Database.sdf");
            File.Copy(_localPath+"\\tempUpdateFiles\\Database.sdf", _localPath + "\\Database.sdf");
        }

        /// <summary>
        /// Downloads all update files from server and applies updates for next startup. 
        /// WARNING: Takes a long time.
        /// </summary>
        public async void DownloadAndUpdateFromServer()
        {
            string localTempFolderPath = _localPath + "\\tempUpdateFiles\\";
            Directory.CreateDirectory(localTempFolderPath);
            SaveDatabase();

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
                        string localFilePath = localTempFolderPath + filenames[i].ToString();
                        ftpClient.DownloadFile(serverFilePath, localFilePath);
                    }
                }
            }

            //Let Squirrel.Windows take over from here
            using (var mgr = new UpdateManager(localTempFolderPath))
            {
                await mgr.UpdateApp();
            }
            ReloadDatabase();
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
        public void DeletePartiallyDownloadedUpdateFiles(object src, EventArgs e)
        {
            if (Directory.Exists(_localPath + "\\tempUdpdateFiles\\"))
            {
                ReloadDatabase();
                Directory.Delete(_localPath + "\\tempUdpdateFiles\\", true);
            }
        }
    }
}

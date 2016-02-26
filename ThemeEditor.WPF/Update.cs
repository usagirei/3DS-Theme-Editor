using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Newtonsoft.Json.Linq;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF
{
    public static class Update
    {
        public static Version AppVersion => Assembly.GetEntryAssembly().GetName().Version;

        public static Version UpdateVersion { get; private set; } = AppVersion;
        public static string UpdateUrl => @"https://api.github.com/repos/usagirei/3ds-theme-editor/releases/latest";

        public static async Task<bool> CheckUpdateAvailable()
        {
            UpdateVersion = await CheckLatestVersion();
            return UpdateVersion > AppVersion;
        }

        public static string UpdatePayloadUrl { get; private set; }

        public static async Task<Version> CheckLatestVersion()
        {
            try
            {
                UpdatePayloadUrl = null;
                UpdateVersion = AppVersion;

                var req = WebRequest.CreateHttp(UpdateUrl);
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                req.KeepAlive = false;
                req.UserAgent = "Mozilla/5.0";

                var myResp = (HttpWebResponse) await req.GetResponseAsync();
                if (myResp.StatusCode == HttpStatusCode.OK)
                {
                    using (var str = myResp.GetResponseStream())
                    using (var rd = new StreamReader(str, Encoding.GetEncoding(myResp.CharacterSet)))
                    {
                        var json = await rd.ReadToEndAsync();
                        var jobj = JObject.Parse(json);
                        var versionStr = jobj["tag_name"].Value<string>();
                        var assets = jobj["assets"];
                        var asset =
                            assets.FirstOrDefault(
                                                  tk =>
                                                  tk["browser_download_url"].Value<string>().
                                                                             EndsWith(versionStr + ".zip",
                                                                                 StringComparison.OrdinalIgnoreCase));
                        if (asset != null)
                            UpdatePayloadUrl = asset["browser_download_url"].Value<string>();

                        versionStr = versionStr.StartsWith("v") ? versionStr.Substring(1) : versionStr;
                        Version version;
                        if (Version.TryParse(versionStr, out version))
                            return version;
                    }
                }
            }
            catch 
            {
                // Ignore
            }
            return null;
        }

        public static async Task<TempFile> DownloadUpdatePayload(Action<long,long> callback)
        {
            if (string.IsNullOrEmpty(UpdatePayloadUrl))
                return null;

            try
            {
                const int CHUNK_SIZE = 4096;

                var req = WebRequest.CreateHttp(UpdatePayloadUrl);
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                req.UserAgent = "Mozilla/5.0";

                byte[] chunk = new byte[CHUNK_SIZE];
                var tf = new TempFile("zip");
                
                var myResp = (HttpWebResponse) await req.GetResponseAsync();
                if (myResp.StatusCode == HttpStatusCode.OK)
                {
                    using(var fStr = File.Open(tf.FilePath, FileMode.Create))
                    using (var str = myResp.GetResponseStream())
                    {
                        var tSize = myResp.ContentLength;
                        var dSize = 0;
                        while (dSize < tSize)
                        {
                            var read = await str.ReadAsync(chunk, 0, CHUNK_SIZE);
                            dSize += read;
                            fStr.Write(chunk, 0, read);
                            callback?.Invoke(dSize, tSize);
                        }
                        return tf;
                    }
                }
            }
            catch
            {
                // Ignore
            }
            return null;
        }

        public static void DeleteEmptyDirs(string path)
        {
            var subDirs = Directory.EnumerateDirectories(path);
            foreach (var subDir in subDirs)
            {
                DeleteEmptyDirs(subDir);
            }
            try
            {
                Directory.Delete(path, false);
            }
            catch (IOException)
            {
                // Ignore, Not Empty Directory
            }
        }

        public static bool ApplyUpdatePayload(TempFile tempFile)
        {
            try
            {
                // Try Opening Zip Beforehand
                // Don't start update process if corrupt
                using (tempFile)
                using (var zf = ZipFile.Open(tempFile.FilePath, ZipArchiveMode.Read))
                {
                    // Move Backup Files
                    var cDir = Path.GetDirectoryName(Application.ResourceAssembly.Location);
                    var oldFiles = Directory.GetFiles(cDir, "*", SearchOption.AllDirectories)
                                            .Select(s => s.Substring(cDir.Length + 1))
                                            .Where(
                                                   s =>
                                                   !Path.GetFileName(s).Equals("config.xml", StringComparison.OrdinalIgnoreCase))
                                            .Where(s => !s.StartsWith("bak_v"))
                                            .ToArray();

                    var nDir = Path.Combine(cDir, "bak_v" + AppVersion);
                    var nFiles = oldFiles.Select(s => new
                    {
                        Source = Path.Combine(cDir, s),
                        Target = Path.Combine(nDir, s)
                    })
                                         .ToArray();

                    // Move To Backup Location
                    foreach (var file in nFiles)
                    {
                        var dir = Path.GetDirectoryName(file.Target);
                        Directory.CreateDirectory(dir);
                        if (File.Exists(file.Target))
                            File.Delete(file.Target);
                        File.Move(file.Source, file.Target);
                    }

                    var cleanDirs = Directory.EnumerateDirectories(cDir)
                                             .Where(s => !Path.GetFileName(s).StartsWith("bak_v"));
                    foreach (var sDir in cleanDirs)
                        DeleteEmptyDirs(sDir);


                    // Extract Update
                    zf.ExtractToDirectory(cDir);
                    return true;
                }
            }
            catch
            {
                return false;
                // Ignore
            }
        }
    
}
}

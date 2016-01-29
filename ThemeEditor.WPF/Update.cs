using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ThemeEditor.WPF
{
    public static class Update
    {
        public static Version AppVersion => Assembly.GetEntryAssembly().GetName().Version;

        public static Version OnlineVersion { get; private set; } = AppVersion;
        public static string UpdateUrl => @"https://github.com/usagirei/3DS-Theme-Editor/releases/latest";

        public static async Task<bool> CheckUpdateAvailable()
        {
            OnlineVersion = await Task<Version>.Factory.StartNew(GetLatestVersion);
            return OnlineVersion > AppVersion;
        }

        public static Version GetLatestVersion()
        {
            var req = WebRequest.CreateHttp(UpdateUrl);
            req.Method = "HEAD";
            req.AllowAutoRedirect = true;

            var myResp = (HttpWebResponse) req.GetResponse();
            if (myResp.StatusCode == HttpStatusCode.OK)
            {
                var latestReleaseUrl = myResp.ResponseUri.ToString().TrimEnd('/');
                var lastSlash = latestReleaseUrl.LastIndexOf('/');
                var versionStr = latestReleaseUrl.Substring(lastSlash + 1);
                versionStr = versionStr.StartsWith("v") ? versionStr.Substring(1) : versionStr;
                Version version;
                if (Version.TryParse(versionStr, out version))
                    return version;
            }
            return null;
        }
    }
}

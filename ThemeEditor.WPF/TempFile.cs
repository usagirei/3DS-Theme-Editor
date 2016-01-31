using System;
using System.IO;

namespace ThemeEditor.WPF
{
    public class TempFile : IDisposable
    {
        public string FilePath { get; }

        public TempFile(string extension)
        {
            var tPath = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            FilePath = Path.Combine(tPath, guid + "." + extension);
            //File.Create(FilePath);
        }

        public void Dispose()
        {
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
            }
            catch (IOException)
            {
                // Ignore
            }
        }
    }
}
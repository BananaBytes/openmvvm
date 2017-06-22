namespace OpenMVVM.Android.PlatformServices
{
    using System;
    using System.IO;

    using OpenMVVM.Core.PlatformServices;

    public class FileServices : IFileServices
    {
        public void WriteFile(string filename, string content)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.WriteAllText(filePath, content);
        }

        public string ReadFile(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.ReadAllText(filePath);
        }

        public DateTime GetCreationTimeUtc(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.GetLastAccessTimeUtc(filePath);
        }
    }
}
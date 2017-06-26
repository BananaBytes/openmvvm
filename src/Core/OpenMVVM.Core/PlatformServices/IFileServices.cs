namespace OpenMVVM.Core.PlatformServices
{
    using System;

    public interface IFileServices
    {
        void WriteFile(string filename, string content);

        string ReadFile(string filename);

        DateTime GetCreationTimeUtc(string filename);
    }
}

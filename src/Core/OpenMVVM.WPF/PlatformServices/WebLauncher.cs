namespace OpenMVVM.WPF.PlatformServices
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    public class WebLauncher : IWebLauncher
    {
        public Task<bool> TryOpenUri(string uri)
        {
            try
            {
                Process.Start(uri);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}

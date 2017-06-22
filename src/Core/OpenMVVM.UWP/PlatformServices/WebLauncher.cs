namespace OpenMVVM.UWP.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    public class WebLauncher : IWebLauncher
    {
        public async Task<bool> TryOpenUri(string uri)
        {
            Uri uriResult;

            var isValid = Uri.TryCreate(uri, UriKind.Absolute, out uriResult);
            if (isValid)
            {
                return await global::Windows.System.Launcher.LaunchUriAsync(uriResult);
            }

            return false;
        }
    }
}
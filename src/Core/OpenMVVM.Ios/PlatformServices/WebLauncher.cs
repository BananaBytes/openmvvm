namespace OpenMVVM.Ios.PlatformServices
{
    using System.Threading.Tasks;

    using Foundation;

    using OpenMVVM.Core.PlatformServices;

    using UIKit;

    public class WebLauncher : IWebLauncher
    {
        public Task<bool> TryOpenUri(string uri)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(uri));

            return Task.FromResult(true);
        }
    }
}
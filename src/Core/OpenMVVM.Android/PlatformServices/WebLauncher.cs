namespace OpenMVVM.Android.PlatformServices
{
    using System.Threading.Tasks;

    using global::Android.Content;

    using OpenMVVM.Core.PlatformServices;

    public class WebLauncher : IWebLauncher
    {
        public Task<bool> TryOpenUri(string url)
        {
            var uri = global::Android.Net.Uri.Parse(url);
            var intent = new Intent(Intent.ActionView, uri);
            AndroidHelpers.CurrentActivity.StartActivity(intent);

            return Task.FromResult(true);
        }
    }
}
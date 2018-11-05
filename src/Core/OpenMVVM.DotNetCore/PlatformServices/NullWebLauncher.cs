using System.Threading.Tasks;
using OpenMVVM.Core.PlatformServices;

namespace OpenMVVM.DotNetCore.PlatformServices
{
    public class NullWebLauncher : IWebLauncher
    {
        public Task<bool> TryOpenUri(string uri)
        {
            return Task.FromResult(true);
        }
    }
}
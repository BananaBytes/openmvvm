namespace OpenMVVM.Core.PlatformServices
{
    using System.Threading.Tasks;

    public interface IWebLauncher
    {
        Task<bool> TryOpenUri(string uri);
    }
}

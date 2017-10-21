namespace OpenMVVM.DotNetCore
{
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    public class NullContentDialogService : IContentDialogService
    {
        public Task Alert(string title, string message)
        {
            return Task.FromResult(0);
        }
    }
}
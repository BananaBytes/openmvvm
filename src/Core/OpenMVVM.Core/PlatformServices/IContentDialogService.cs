namespace OpenMVVM.Core.PlatformServices
{
    using System.Threading.Tasks;

    public interface IContentDialogService
    {
        Task Alert(string title, string message);
    }
} 
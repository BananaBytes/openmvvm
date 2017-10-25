namespace OpenMVVM.WPF.PlatformServices
{
    using System.Threading.Tasks;
    using System.Windows;

    using OpenMVVM.Core.PlatformServices;

    public class ContentDialogService : IContentDialogService
    {
        public async Task Alert(string title, string message)
        {
            await Application.Current.Dispatcher.InvokeAsync(
                () =>
                    {
                        MessageBox.Show(message, title);
                    });
        }
    }
}

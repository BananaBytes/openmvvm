namespace OpenMVVM.UWP.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    using global::Windows.UI.Popups;

    public class ContentDialogService : IContentDialogService
    {
        private readonly IDispatcherService dispatcherService;

        public ContentDialogService(IDispatcherService dispatcherService)
        {
            this.dispatcherService = dispatcherService;
        }

        public async Task Alert(string title, string message)
        {
            await this.dispatcherService.RunAsync(async () =>
                     {
                         var dialog = new MessageDialog(message, title);
                         await dialog.ShowAsync();
                     });
        }
    }
}
namespace OpenMVVM.Ios.PlatformServices
{
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    using UIKit;

    public class ContentDialogService : IContentDialogService
    {
        private readonly IDispatcherService dispatcherService;

        public ContentDialogService(IDispatcherService dispatcherService)
        {
            this.dispatcherService = dispatcherService;
        }

        public Task Alert(string title, string message)
        {
            this.dispatcherService.Run(
                () =>
                    {
                        UIAlertView alert = new UIAlertView() { Title = title, Message = message };

                        alert.AddButton("OK");
                        alert.Show();
                    });

            return Task.CompletedTask;
        }
    }
}
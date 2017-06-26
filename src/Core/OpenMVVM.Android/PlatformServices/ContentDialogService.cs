namespace OpenMVVM.Android.PlatformServices
{
    using System.Threading.Tasks;

    using global::Android.App;

    using OpenMVVM.Core.PlatformServices;

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
                        AlertDialog.Builder alert = new AlertDialog.Builder(AndroidHelpers.CurrentActivity);
                        alert.SetTitle(title);
                        alert.SetMessage(message);
                        alert.SetPositiveButton("OK", (senderAlert, args) => { });

                        Dialog dialog = alert.Create();
                        dialog.Show();
                    });

            return Task.CompletedTask;
        }
    }
}
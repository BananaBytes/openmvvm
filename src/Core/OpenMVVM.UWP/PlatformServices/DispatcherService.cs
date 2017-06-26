namespace OpenMVVM.UWP.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    using global::Windows.ApplicationModel.Core;
    using global::Windows.UI.Core;

    public class DispatcherService : IDispatcherService
    {
        public async void Run(Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }

        public async Task RunAsync(Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }
    }
}

namespace OpenMVVM.Android.PlatformServices
{
    using System;
    using System.Threading.Tasks;
    using OpenMVVM.Core.PlatformServices;

    public class DispatcherService : IDispatcherService
    {
        public void Run(Action action)
        {
            AndroidHelpers.CurrentActivity.RunOnUiThread(action);
        }

        public Task RunAsync(Action action)
        {
            throw new NotImplementedException();
        }
    }
}
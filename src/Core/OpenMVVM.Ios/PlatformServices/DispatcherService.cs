namespace OpenMVVM.Ios.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    using UIKit;

    public class DispatcherService : IDispatcherService
    {
        public void Run(Action action)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(action);
        }

        public Task RunAsync(Action action)
        {
            throw new System.NotImplementedException();
        }
    }
}
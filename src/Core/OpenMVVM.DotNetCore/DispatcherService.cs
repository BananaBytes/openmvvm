namespace OpenMVVM.DotNetCore
{
    using System;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;

    public class DispatcherService : IDispatcherService
    {
        public void Run(Action action)
        {
            action();
        }

        public Task RunAsync(Action action)
        {
            return Task.Run(action);
        }
    }
}

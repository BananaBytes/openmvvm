namespace OpenMVVM.Core.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    public interface IDispatcherService
    {
        void Run(Action action);

        Task RunAsync(Action action);
    }
}
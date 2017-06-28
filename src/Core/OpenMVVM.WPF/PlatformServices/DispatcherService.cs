namespace OpenMVVM.WPF.PlatformServices
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    using OpenMVVM.Core.PlatformServices;

    public class DispatcherService : IDispatcherService
    { 
        public void Run(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher.CheckAccess())
            {
                action.Invoke();
            }
            else
            {
                dispatcher.BeginInvoke(action);
            }
        }

        public async Task RunAsync(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                await dispatcher.InvokeAsync(action);
            }
        }
    }
}   
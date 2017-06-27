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

        public Task RunAsync(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher.CheckAccess())
            {
                return Task.Run(action);
            }
            else
            {
                return dispatcher.InvokeAsync(action).Task;
            }
        }
    }
}   
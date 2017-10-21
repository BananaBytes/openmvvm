namespace OpenMVVM.WebView.Web.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices;
    public class NullContentDialogService : IContentDialogService
    {
        public Task Alert(string title, string message)
        {
            return Task.FromResult(0);
        }
    }

    public class NullDescriptionService : IDescriptionService
    {
        public string Platform {
            get
            {
                return "DotNetCore";
            }
        }
    }

    public class NullDispatcherService : IDispatcherService
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

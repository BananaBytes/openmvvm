namespace OpenMVVM.WebView.Web.PlatformServices
{
    using OpenMVVM.Core.PlatformServices.Lifecycle;

    public class LifecycleService : ILifecycleService
    {
        public bool TryCloseApplication()
        {
            return true;
        }
    }
}

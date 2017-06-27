namespace OpenMVVM.WPF.PlatformServices
{
    using System.Windows;

    using OpenMVVM.Core.PlatformServices.Lifecycle;

    public class LifecycleService : ILifecycleService
    {
        public bool TryCloseApplication()
        {
            Application.Current.Shutdown();

            return true;
        }
    }
}

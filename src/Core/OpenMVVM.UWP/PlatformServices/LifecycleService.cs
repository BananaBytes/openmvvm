namespace OpenMVVM.UWP.PlatformServices
{
    using global::Windows.UI.Xaml;

    using OpenMVVM.Core.PlatformServices.Lifecycle;

    public class LifecycleService : ILifecycleService
    {
        public bool TryCloseApplication()
        {
            Application.Current.Exit();

            return true;
        }
    }
}

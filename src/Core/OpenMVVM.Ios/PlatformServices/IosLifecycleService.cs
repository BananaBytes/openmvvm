namespace OpenMVVM.Ios.PlatformServices
{
    using OpenMVVM.Core.PlatformServices.Lifecycle;

    public class IosLifecycleService : ILifecycleService
    {
        public bool TryCloseApplication()
        {
            return true;
        }
    }
}

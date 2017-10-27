namespace OpenMVVM.DotNetCore
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

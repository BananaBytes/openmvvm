namespace OpenMVVM.Android.PlatformServices
{
    using System;

    using global::Android.OS;

    using OpenMVVM.Core.PlatformServices.Lifecycle;

    public class LifecycleService : ILifecycleService
    {
        public bool TryCloseApplication()
        {
            try
            {
                Process.KillProcess(Process.MyPid());
                return true;
            }
            catch (Exception) { }
            
            return false;
        }
    }
}
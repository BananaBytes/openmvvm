using System;
using Android.OS;
using OpenMVVM.Core.PlatformServices.Lifecycle;

namespace OpenMVVM.Android.PlatformServices
{
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
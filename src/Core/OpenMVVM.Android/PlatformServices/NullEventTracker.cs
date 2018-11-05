namespace OpenMVVM.Android.PlatformServices
{
    using OpenMVVM.Core.PlatformServices;

    public class NullEventTracker : IEventTracker
    {
        public void TrackEvent(string eventDescription)
        {            
        }
    }
}
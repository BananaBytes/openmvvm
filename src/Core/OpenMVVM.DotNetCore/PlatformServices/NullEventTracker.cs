using OpenMVVM.Core.PlatformServices;

namespace OpenMVVM.DotNetCore.PlatformServices
{
    public class NullEventTracker : IEventTracker
    {
        public void TrackEvent(string eventDescription)
        {
        }
    }
}
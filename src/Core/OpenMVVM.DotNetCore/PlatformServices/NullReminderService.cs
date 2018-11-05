using System.Threading.Tasks;
using OpenMVVM.Core.PlatformServices;

namespace OpenMVVM.DotNetCore.PlatformServices
{
    public class NullReminderService : IReminderService
    {
        public Task<bool> HasReminderAsync(string id)
        {
            return Task.FromResult(false);
        }

        public Task<bool> AddReminderAsync(string id, CalendarEvent calEvent)
        {
            return Task.FromResult(false);
        }

        public Task<bool> RemoveReminderAsync(string id)
        {
            return Task.FromResult(false);
        }
    }
}
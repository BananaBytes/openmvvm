namespace OpenMVVM.Core.PlatformServices
{
    using System.Threading.Tasks;

    public interface IReminderService
    {
        Task<bool> HasReminderAsync(string id);

        Task<bool> AddReminderAsync(string id, CalendarEvent calEvent);

        Task<bool> RemoveReminderAsync(string id);
    }
}

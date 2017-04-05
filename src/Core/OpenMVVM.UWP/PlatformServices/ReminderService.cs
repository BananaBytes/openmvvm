namespace OpenMVVM.UWP.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using global::Windows.Foundation;
    using global::Windows.UI.Popups;

    using OpenMVVM.Core.PlatformServices;

    public class ReminderService : IReminderService
    {
        public Task<bool> HasReminderAsync(string id)
        {
            return Task.FromResult(false);
        }

        public async Task<bool> AddReminderAsync(string id, CalendarEvent calEvent)
        {
            var appointment = new global::Windows.ApplicationModel.Appointments.Appointment();
            appointment.StartTime = calEvent.Start;

            appointment.Subject = calEvent.Name;
            appointment.Details = calEvent.Description;
            appointment.Location = calEvent.Location;
            appointment.AllDay = calEvent.AllDay;

            await global::Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(
                    appointment,
                    Rect.Empty,
                    Placement.Default);

            return true;
        }

        public Task<bool> RemoveReminderAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}

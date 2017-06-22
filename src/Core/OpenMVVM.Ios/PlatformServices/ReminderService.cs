namespace OpenMVVM.Ios.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using EventKit;

    using Foundation;

    using OpenMVVM.Core.PlatformServices;

    public class ReminderService : IReminderService
    {
        private static readonly EKEventStore EventStore = new EKEventStore();

        private readonly IContentDialogService contentDialogService;

        public ReminderService(IContentDialogService contentDialogService)
        {
            this.contentDialogService = contentDialogService;
        }

        public Task<bool> AddReminderAsync(string id, CalendarEvent calEvent)
        {
            bool success = true;

            EventStore.RequestAccess(
                EKEntityType.Event,
                (granted, e) =>
                    {
                        if (granted)
                        {
                            var newEvent = EKEvent.FromStore(EventStore);

                            newEvent.AddAlarm(EKAlarm.FromDate(DateTimeToNSDate(calEvent.Start.AddMinutes(-15))));
                            newEvent.StartDate = DateTimeToNSDate(calEvent.Start);
                            newEvent.EndDate = DateTimeToNSDate(calEvent.End);
                            newEvent.Title = calEvent.Name;
                            newEvent.Notes = calEvent.Description;
                            newEvent.Location = calEvent.Location;
                            newEvent.AllDay = calEvent.AllDay;

                            newEvent.Calendar = EventStore.DefaultCalendarForNewEvents;
                        }
                        else
                        {
                            this.contentDialogService.Alert("Access Denied", "User Denied Access to Calendar Data");
                            success = false;
                        }
                    });

            return Task.FromResult(success);
        }

        public Task<bool> HasReminderAsync(string id)
        {
            return Task.FromResult(false);
        }

        public Task<bool> RemoveReminderAsync(string id)
        {
            return Task.FromResult(true);
        }

        private static NSDate DateTimeToNSDate(DateTime date)
        {
            var reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate((date - reference).TotalSeconds);
        }
    }
}
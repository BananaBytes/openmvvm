namespace OpenMVVM.Android.PlatformServices
{
    using System;
    using System.Threading.Tasks;

    using global::Android.Content;
    using global::Android.Provider;

    using Java.Util;

    using OpenMVVM.Core.PlatformServices;

    public class ReminderService : IReminderService
    {
        public Task<bool> HasReminderAsync(string id)
        {
            return Task.FromResult(false);
        }

        public Task<bool> AddReminderAsync(string id, CalendarEvent calEvent)
        {
            var calendarsUri = CalendarContract.Calendars.ContentUri;

            string[] calendarsProjection = {
                CalendarContract.Calendars.InterfaceConsts.Id,
                CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                CalendarContract.Calendars.InterfaceConsts.AccountName
            };

            var cursor = AndroidHelpers.CurrentActivity.ContentResolver.Query(calendarsUri, calendarsProjection, null, null, null);

            cursor.MoveToFirst();
            int calId = cursor.GetInt(cursor.GetColumnIndex(calendarsProjection[0]));

            ContentValues eventValues = new ContentValues();
            eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, calId);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.AllDay, Convert.ToInt32(calEvent.AllDay));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventLocation, calEvent.Location);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, calEvent.Name);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, calEvent.Description);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "Europe/Belgrade");
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, this.GetDateTimeMS(calEvent.Start.Year, calEvent.Start.Month, calEvent.Start.Day, calEvent.Start.Hour, calEvent.Start.Minute));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, this.GetDateTimeMS(calEvent.End.Year, calEvent.End.Month, calEvent.End.Day, calEvent.End.Hour, calEvent.End.Minute));

            var uri = AndroidHelpers.CurrentActivity.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);
            return Task.FromResult(true);
        }

        public Task<bool> RemoveReminderAsync(string id)
        {
            return Task.FromResult(true);
        }

        private long GetDateTimeMS(int yr, int month, int day, int hr, int min)
        {
            var c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(CalendarField.DayOfMonth, day);
            c.Set(CalendarField.HourOfDay, hr + 2);
            c.Set(CalendarField.Minute, min);
            c.Set(CalendarField.Month, month - 1);
            c.Set(CalendarField.Year, yr);

            return c.TimeInMillis;
        }
    }
}
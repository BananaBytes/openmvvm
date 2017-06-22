namespace OpenMVVM.Core.PlatformServices
{
    using System;

    public class CalendarEvent
    {
        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Location { get; set; }

        public bool AllDay { get; set; }

        public string Description { get; set; }

        public string ExternalId { get; set; }
    }
}

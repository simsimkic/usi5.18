using System;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Domain
{
    public class TimeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Duration { get; set; }
        public static int INTERVAL = 15;


        [JsonConstructor]
        public TimeSlot(DateTime start)
        {
            Start = start;
            End = start.AddMinutes(INTERVAL);
            Duration = INTERVAL;
        }

        public TimeSlot(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
            Duration = (end - start).Minutes;
        }

        public bool OverlapsWithTimeSlot(TimeSlot otherTimeSlot)
        {
            if (Start <= otherTimeSlot.Start && End > otherTimeSlot.Start)
                return true;
            if (Start >= otherTimeSlot.Start && Start < otherTimeSlot.End)
                return true;
            return false;
        }
    }
}

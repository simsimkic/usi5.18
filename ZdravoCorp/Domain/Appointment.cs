using System;
using System.Text.Json.Serialization;
using ZdravoCorp.Domain.UserClasses;

namespace ZdravoCorp.Domain
{
    public class Appointment
    {
        public enum AppointmentStatus
        {
            Booked,
            Compleated,
            Canceled,
        }

        public int Id { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public Doctor Doctor{ get; set; }
        public MedicalRecord MedicalRecord{ get; set; }

        public String Type { get; set; }
        public int RoomId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentStatus Status { get; set; }

        public Appointment(int id, TimeSlot timeSlot, Doctor doctor, MedicalRecord medicalRecord, AppointmentStatus status, String type, int roomId)
        {
            Id = id;
            this.TimeSlot = timeSlot;
            this.Doctor = doctor;
            this.MedicalRecord = medicalRecord;
            this.Status = status;
            Type = type;
            this.RoomId = roomId;
        }
    }
}

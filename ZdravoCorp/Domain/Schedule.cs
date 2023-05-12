using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Domain.UserClasses;
using static ZdravoCorp.Domain.Appointment;

namespace ZdravoCorp.Domain
{
    public class Schedule
    {
        private List<Appointment> _appointments;
        public String path = @"..\\..\\..\\Data\\appointments.json";

        public List<Appointment> GetAppointments()
        {
            return _appointments;
        }

        public void LoadAppointmentsFromJson()
        {
            string jsonData = File.ReadAllText(this.path);
            _appointments = JsonSerializer.Deserialize<List<Appointment>>(jsonData);
        }

        public int GenerateID()
        {
            List<int> allIds = GetAppointmentIds();
            int randomID;
            do
            {
                randomID = new Random().Next(1, 10001);
            } while (allIds.Contains(randomID));

            return randomID;
        }

        public List<int> GetAppointmentIds()
        {
            List<int> idList = new List<int>();
            LoadAppointmentsFromJson();
            foreach (Appointment ap in _appointments) { idList.Add(ap.Id); }
            return idList;
        }

        public void WriteToJson()
        {
            string jsonData = JsonSerializer.Serialize(_appointments);
            File.WriteAllText(path, jsonData);
        }


        public bool IsAvailable(Doctor doctor, TimeSlot time)
        {
            List<Appointment> doctorAppointments = GetAppointmentsByDoctorId(doctor.Id);
            foreach (Appointment appointment in doctorAppointments)
            {
                if (appointment.TimeSlot.OverlapsWithTimeSlot(time)) return false;
            }
            return true;
        }

        public bool IsAvailable(Patient patient, TimeSlot time)
        {
            foreach (Appointment appointment in GetAppointments().Where(x => x.MedicalRecord.Patient.Id == patient.Id).ToList())
            {
                if (appointment.TimeSlot.OverlapsWithTimeSlot(time)) return false;
            }
            return true;
        }


        public void UpdateAppointment(int id, TimeSlot timeSlot, Doctor doctor, MedicalRecord medicalRecord, AppointmentStatus status, String type, int roomId)
        {
            if (AppointmentExists(id))
            {
                Appointment appointmentToUpdate = GetAppointmentById(id);
                appointmentToUpdate.TimeSlot = timeSlot;
                appointmentToUpdate.Doctor = doctor;
                appointmentToUpdate.MedicalRecord = medicalRecord;
                appointmentToUpdate.Status = status;
                appointmentToUpdate.Type = type;
                WriteToJson();
            }
        }

        public void CancelAppointment(int appointmentId)
        {
            if (AppointmentExists(appointmentId))
            {
                Appointment appointmentToCancel = GetAppointmentById(appointmentId);
                appointmentToCancel.Status = Appointment.AppointmentStatus.Canceled;
                WriteToJson();
            }
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            if (AppointmentExists(appointmentId))
                return _appointments.Find(x => x.Id == appointmentId);
            return null;
        }

        public bool AppointmentExists(int appointmentId)
        {
            return _appointments.Any(x => x.Id == appointmentId);
        }

        public List<Appointment> GetAppointmentsByDoctorId(int id)
        {
            return _appointments.Where(x => x.Doctor.Id == id).ToList();
        }

        public List<Appointment> GetDoctorAppointments(Doctor doc)
        {
            return _appointments.Where(x => x.Doctor.Id == doc.Id).ToList();
        }

        public void NewAppointment(Appointment newAppointment)
        {
            _appointments.Add(newAppointment);
            WriteToJson();
        }

        public List<Appointment> GetAppointmentsFor3Days(List<Appointment> doctorsAppoinntments, DateTime startDate)
        {
            DateTime endDate = startDate.AddDays(3);
            return doctorsAppoinntments.Where(x => x.TimeSlot.Start >= startDate && x.TimeSlot.Start <= endDate && x.Status == Appointment.AppointmentStatus.Booked) .ToList();
        }

        public List<Appointment> GetAppointmentsForToday(List<Appointment> doctorsAppoinntments)
        {
            return doctorsAppoinntments.Where(x => x.TimeSlot.Start.Date >= DateTime.Today  && x.TimeSlot.End.Date <= DateTime.Today.AddDays(1).Date && x.Status == Appointment.AppointmentStatus.Booked && x.Type=="examination") .ToList();
        }
    }
}

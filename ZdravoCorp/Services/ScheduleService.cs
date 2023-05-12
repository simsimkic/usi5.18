using System;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using System.Collections.Generic;

namespace ZdravoCorp.Services;

public class ScheduleService
{
    private Schedule _schedule;

    public ScheduleService()
    {
        _schedule = new Schedule();
        _schedule.LoadAppointmentsFromJson();
    }

    public bool IsAvailable(Doctor doctor, TimeSlot time)
    {
        return _schedule.IsAvailable(doctor, time);
    }



    public bool IsAvailable(Patient patient, TimeSlot time)
    {
        return _schedule.IsAvailable(patient, time);
    }

    public void NewAppointment(Appointment appointment)
    {
        _schedule.NewAppointment(appointment);
    }
    
    public void UpdateAppointment(int id, TimeSlot timeSlot, Doctor doctor, MedicalRecord medicalRecord,
        Appointment.AppointmentStatus status, String type, int roomId)
    {
        _schedule.UpdateAppointment(id, timeSlot, doctor, medicalRecord, status, type, roomId);
    }

    public void CancelAppointment(int appoitmentId)
    {
        _schedule.CancelAppointment(appoitmentId);
    }

    public Appointment GetAppointmentById(int appointmentId)
    {
        return _schedule.GetAppointmentById(appointmentId);
    }

    public int GenerateAppointmentId()
    {
        return _schedule.GenerateID();
    }
    public List<Appointment> GetAllAppointments()
    {
        return _schedule.GetAppointments();
    }
    public List<Appointment> GetAppointmentsByDoctorId(int id)
    {
        return _schedule.GetAppointmentsByDoctorId(id);
    }

    public List<Appointment> GetAppointmentsForToday(List<Appointment> doctorAppointments)
    {
        return _schedule.GetAppointmentsForToday(doctorAppointments);
    }

    public List<Appointment> GetDoctorAppointments(Doctor doc)
    {
        return _schedule.GetDoctorAppointments(doc);
    }

    public List<Appointment> GetAppointmentsFor3Days(List<Appointment> doctorsAppoinntments, DateTime startDate)
    {
        return _schedule.GetAppointmentsFor3Days(doctorsAppoinntments, startDate);
    }

    public void Refresh()
    {
        _schedule.LoadAppointmentsFromJson();
    }
}
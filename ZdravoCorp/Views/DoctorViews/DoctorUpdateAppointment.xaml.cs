using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    /// <summary>
    /// Interaction logic for UpdateAppointment.xaml
    /// </summary>
    public partial class UpdateAppointment : Window
    {
        private DoctorService _doctorService = new DoctorService();
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private ScheduleService _scheduleService = new ScheduleService();
        private PatientService _patientService = new PatientService();
        private int _idToUpdate;
        public UpdateAppointment(Appointment appointment)
        {
            InitializeComponent();
            _patientService.Refresh();
            List<Patient> patientList = _patientService.GetAllPatients();
            foreach (Patient patient in patientList) { cbPatients.Items.Add(string.Join(",", patient.Id, patient.FirstName, patient.LastName)); }
            _idToUpdate = appointment.Id;
        }

        private void miLogout_Click(object sender, RoutedEventArgs e){
            LoginScreen ls = new LoginScreen();
            ls.Show();
            this.Close();
        }

        private void ViewObligation_OnClick(object sender, RoutedEventArgs e)
        {
            ViewAppointment vp = new ViewAppointment();
            vp.Show();
            this.Close();
        }

        private void NewObligation_OnClick(object sender, RoutedEventArgs e)
        {
            NewAppointment na = new NewAppointment();
            na.Show();
            this.Close();
        }

        private void CancelObligation_OnClick(object sender, RoutedEventArgs e)
        {
            CancelAppointment ca = new CancelAppointment();
            ca.Show();
            this.Close();
        }
        private void StartingPage_OnClick(object sender, RoutedEventArgs e)
        {
            DoctorLandingPage dlp = new DoctorLandingPage();
            dlp.Show();
            this.Close();
        }


        private void RbExamination_OnChecked(object sender, RoutedEventArgs e)
        {

            dpEndPicker.IsEnabled = false;
            cbEndHours.IsEnabled = false;
            cbEndMinutes.IsEnabled = false;
        }
        private void RbSurgery_OnChecked(object sender, RoutedEventArgs e)
        {
            dpEndPicker.IsEnabled = true;
            cbEndHours.IsEnabled = true;
            cbEndMinutes.IsEnabled = true;
        }

        public void ClearFields()
        {
            cbPatients.SelectedIndex = -1;
            dpStartPicker.SelectedDate = null;
            dpEndPicker.SelectedDate = null;
            cbStartHours.SelectedIndex = -1;
            cbEndHours.SelectedIndex = -1;
            cbStartMinutes.SelectedIndex = -1;
            cbEndMinutes.SelectedIndex = -1;
        }

        private void btnUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            _scheduleService.Refresh();
            if (rbExamination.IsChecked == false && rbSurgery.IsChecked == false)
            {
                MessageBox.Show("You need to chose a type.");
            }
            else
            {
                if (rbSurgery.IsChecked == true)
                {
                    string type = "surgery";
                    if (cbPatients.SelectedIndex == -1 || dpStartPicker.SelectedDate == null ||
                        dpEndPicker.SelectedDate == null || cbStartHours.SelectedIndex == -1 ||
                        cbEndHours.SelectedIndex == -1 || cbStartMinutes.SelectedIndex == -1 ||
                        cbEndMinutes.SelectedIndex == -1)
                    {
                        MessageBox.Show("All fields are required");
                    }
                    else
                    {
                        _doctorService.Refresh();
                        _medicalRecordService.Refresh();
                        _patientService.Refresh();
                        Random random = new Random();

                        Doctor doc = _doctorService.DoctorCookie();
                        Patient patient = _patientService.GetPatientById(int.Parse(cbPatients.Text.Split(",")[0]));
                        TimeSlot time = new TimeSlot(
                            DateTime.Parse(dpStartPicker.Text + " " + cbStartHours.Text + ":" + cbStartMinutes.Text),
                            DateTime.Parse(dpEndPicker.Text + " " + cbEndHours.Text + ":" + cbEndMinutes.Text));
                        MedicalRecord mr = _medicalRecordService.GetMedicalRecordByPatientId(patient.Id);

                        if (time.Start >= DateTime.Now && time.End >= time.Start)
                        {
                            if (_scheduleService.IsAvailable(doc, time) && _scheduleService.IsAvailable(patient, time))
                            {
                                Appointment appointment = new Appointment(_idToUpdate,time, doc, mr, Appointment.AppointmentStatus.Booked, type, new AppointmentRespository().AssignRoom(RoomType.OperatingRoom));
                                _scheduleService.UpdateAppointment(_idToUpdate, time, doc, mr, Appointment.AppointmentStatus.Booked, type, appointment.RoomId);
                                MessageBox.Show("Appointment update.");
                            }
                            else
                            {
                                MessageBox.Show("Patient or doctor unavailable.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("You cant choose a date in the past.");
                        }

                    }
                }
                else
                {
                    string type = "examination";

                    if (cbPatients.SelectedIndex == -1 || dpStartPicker.SelectedDate == null ||
                         cbStartHours.SelectedIndex == -1 || cbStartMinutes.SelectedIndex == -1)
                    {
                        MessageBox.Show("All fields are required");
                    }
                    else
                    {
                        _doctorService.Refresh();
                        _medicalRecordService.Refresh();
                        _patientService.Refresh();
                        Random random = new Random();

                        Doctor doc = _doctorService.DoctorCookie();
                        Patient patient = _patientService.GetPatientById(int.Parse(cbPatients.Text.Split(",")[0]));
                        TimeSlot time = new TimeSlot( DateTime.Parse(dpStartPicker.Text + " " + cbStartHours.Text + ":" + cbStartMinutes.Text));
                        MedicalRecord mr = _medicalRecordService.GetMedicalRecordByPatientId(patient.Id);
                        if (time.Start >= DateTime.Now)
                        {
                            if (_scheduleService.IsAvailable(doc, time) && _scheduleService.IsAvailable(patient, time))
                            {
                                Appointment appointment = new Appointment(_idToUpdate,time, doc, mr, Appointment.AppointmentStatus.Booked,type, new AppointmentRespository().AssignRoom(RoomType.ExaminationRoom));
                                _scheduleService.UpdateAppointment(_idToUpdate, time, doc, mr, Appointment.AppointmentStatus.Booked, type, appointment.RoomId);
                                MessageBox.Show("Appointment updated.");
                            }
                            else
                            {
                                MessageBox.Show("Patient or doctor unavailable.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("You can't choose a date in the past.");
                        }
                    }
                }
            }

        }

        private void miSearch_Click(object sender, RoutedEventArgs e)
        {
            DoctorPatientSearch dps = new DoctorPatientSearch();
            dps.Show();
            this.Close();
        }

        private void miOverview_OnClick(object sender, RoutedEventArgs e)
        {
            DoctorAllExaminations dae = new DoctorAllExaminations();
            dae.Show();
            this.Close();
        }
    }
}

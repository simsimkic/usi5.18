using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Data;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    /// <summary>
    /// Interaction logic for DoctorLandingPage.xaml
    /// </summary>
    public partial class DoctorLandingPage : Window
    {
        private DoctorService _doctorService = new DoctorService();
        private ScheduleService _scheduleService = new ScheduleService();
        public DoctorLandingPage()
        {
            InitializeComponent();
            Doctor loggedInAs = _doctorService.DoctorCookie();
            _scheduleService.Refresh();
            lblHello.Content += " " + loggedInAs.FirstName + " " + loggedInAs.LastName + "!";
            lblRole.Content += " " + loggedInAs.Role + ", " + loggedInAs.Specialty;
            LoadLandingDataGrid(loggedInAs);
        }

        void LoadLandingDataGrid(Doctor doctor)
        {
            DataTable dt = new DataTable();
            DataColumn id = new DataColumn("ID", typeof(int)); dt.Columns.Add(id);
            DataColumn firstname = new DataColumn("First Name", typeof(String)); dt.Columns.Add(firstname);
            DataColumn lastname = new DataColumn("Last Name", typeof(String)); dt.Columns.Add(lastname);
            DataColumn startTime = new DataColumn("Start", typeof(String)); dt.Columns.Add(startTime);
            DataColumn endTime = new DataColumn("End", typeof(String)); dt.Columns.Add(endTime);
            DataColumn room = new DataColumn("Room", typeof(String)); dt.Columns.Add(room);
            DataColumn type = new DataColumn("Type", typeof(String)); dt.Columns.Add(type);

            List<Appointment> allAppointments = _scheduleService.GetAppointmentsByDoctorId(doctor.Id);
            List<Appointment> upcomingAppointments = _scheduleService.GetAppointmentsFor3Days(allAppointments, DateTime.Now);
            foreach (Appointment ap in upcomingAppointments)
            {
                DataRow row = dt.NewRow();
                row[0] = ap.Id;
                row[1] = ap.MedicalRecord.Patient.FirstName;
                row[2] = ap.MedicalRecord.Patient.LastName;
                row[3] = ap.TimeSlot.Start.ToString();
                row[4] = ap.TimeSlot.End.ToString();
                row[5] = ap.RoomId.ToString();
                row[6] = ap.Type.ToString();
                dt.Rows.Add(row);
            }
            dgLandingData.ItemsSource = dt.DefaultView;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen ls = new LoginScreen();
            ls.Show();
            this.Close();
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

        private void MiSearch_OnClick(object sender, RoutedEventArgs e)
        {
            DoctorPatientSearch dps = new DoctorPatientSearch();
            dps.Show();
            this.Close();
        }

        private void miWork_OnClick(object sender, RoutedEventArgs e)
        {
            DoctorAllExaminations dae = new DoctorAllExaminations();
            dae.Show();
            this.Close();
        }
    }
}

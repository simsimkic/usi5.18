using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    /// <summary>
    /// Interaction logic for ViewAppointment.xaml
    /// </summary>
    public partial class ViewAppointment : Window
    {
        private DoctorService _doctorService = new DoctorService();
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private ScheduleService _scheduleService = new ScheduleService();
        private PatientService _patientService = new PatientService();
        public ViewAppointment()
        {
            InitializeComponent();
        }
        
        private void btnDateSelect_Click(object sender, RoutedEventArgs e)
        {
            Doctor doc = _doctorService.DoctorCookie();
            _scheduleService.Refresh();
            DateTime selectedDate = dpAppointmentsDate.SelectedDate.GetValueOrDefault();
            if (selectedDate != null)
            {
                LoadDatePickerAppointments(doc, selectedDate);
            }
        }

        public void LoadDatePickerAppointments( Doctor doctor, DateTime time)
        {
            DataTable dataTable = LoadDataTable();
            List<Appointment> allAppointments = _scheduleService.GetAppointmentsByDoctorId(doctor.Id);
            List<Appointment> upcomingAppointments = _scheduleService.GetAppointmentsFor3Days(allAppointments, time);
            foreach (Appointment appointment in upcomingAppointments)
            {
                DataRow row = LoadDataRow(dataTable, appointment);
                dataTable.Rows.Add(row);
            }
            dgDateAppointments.ItemsSource = dataTable.DefaultView;
        }

        public DataTable LoadDataTable()
        {
            DataTable dataTable = new DataTable();
            DataColumn id = new DataColumn("Appointment ID", typeof(int)); dataTable.Columns.Add(id);
            DataColumn patientId = new DataColumn("Patient ID", typeof(int)); dataTable.Columns.Add(patientId);
            DataColumn firstname = new DataColumn("First Name", typeof(String)); dataTable.Columns.Add(firstname);
            DataColumn lastname = new DataColumn("Last Name", typeof(String)); dataTable.Columns.Add(lastname);
            DataColumn startTime = new DataColumn("Start", typeof(String)); dataTable.Columns.Add(startTime);
            DataColumn endTime = new DataColumn("End", typeof(String)); dataTable.Columns.Add(endTime);
            DataColumn status = new DataColumn("Status", typeof(String)); dataTable.Columns.Add(status);
            return dataTable;
        }

        public DataRow LoadDataRow(DataTable dataTable, Appointment appointment)
        {
            DataRow row = dataTable.NewRow();
            row[0] = appointment.Id.ToString();
            row[1] = appointment.MedicalRecord.Patient.Id;
            row[2] = appointment.MedicalRecord.Patient.FirstName;
            row[3] = appointment.MedicalRecord.Patient.LastName;
            row[4] = appointment.TimeSlot.Start.ToString();
            row[5] = appointment.TimeSlot.End.ToString();
            row[6] = appointment.Status.ToString();
            return row;
        }

        private void BtnViewUser_OnClick(object sender, RoutedEventArgs e)
        {
            _medicalRecordService.Refresh();
            _patientService.Refresh();

            if (dgDateAppointments.SelectedIndex == -1)
            {
                MessageBox.Show("Missing ID.");
            }
            else
            {
                var selectedPatient = Convert.ToInt32((dgDateAppointments.SelectedItem as DataRowView).Row.ItemArray[1]);
                MedicalRecord mr = _medicalRecordService.GetMedicalRecordByPatientId(selectedPatient);
                PatientInfo pi = new PatientInfo(mr);
                pi.Show();
                this.Close();
            }
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

        private void MiOverview_OnClick(object sender, RoutedEventArgs e)
        {
            DoctorAllExaminations dae = new DoctorAllExaminations();
            dae.Show();
            this.Close();
        }
    }
}

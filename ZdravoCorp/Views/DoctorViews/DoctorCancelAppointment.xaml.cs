using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    /// <summary>
    /// Interaction logic for CancelAppointment.xaml
    /// </summary>
    public partial class CancelAppointment : Window
    {
        private DoctorService _doctorService = new DoctorService();
        private ScheduleService _scheduleService = new ScheduleService();
        public CancelAppointment()
        {
            InitializeComponent();
            Doctor doctor = _doctorService.DoctorCookie();
            _scheduleService.Refresh();
            LoadCancelAppointments(doctor);
        }

        public void LoadCancelAppointments(Doctor doctor)
        {
            DataTable dataTable = LoadDataTable();
            List<Appointment> allAppointments = _scheduleService.GetAppointmentsByDoctorId(doctor.Id);
            foreach (Appointment appointment in allAppointments)
            {
                DataRow row = LoadDataRow(dataTable, appointment);
                dataTable.Rows.Add(row);
            }
            dgAllAppointments.ItemsSource = dataTable.DefaultView;
        }

        public DataTable LoadDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn id = new DataColumn("ID", typeof(int)); dt.Columns.Add(id);
            DataColumn patientId = new DataColumn("Patient ID", typeof(int)); dt.Columns.Add(patientId);
            DataColumn firstname = new DataColumn("First Name", typeof(String)); dt.Columns.Add(firstname);
            DataColumn lastname = new DataColumn("Last Name", typeof(String)); dt.Columns.Add(lastname);
            DataColumn startTime = new DataColumn("Start", typeof(String)); dt.Columns.Add(startTime);
            DataColumn endTime = new DataColumn("End", typeof(String)); dt.Columns.Add(endTime);
            return dt;
        }

        public DataRow LoadDataRow(DataTable dt, Appointment appointment)
        {
            DataRow row = dt.NewRow();
            row[0] = appointment.Id;
            row[1] = appointment.MedicalRecord.Patient.Id;
            row[2] = appointment.MedicalRecord.Patient.FirstName;
            row[3] = appointment.MedicalRecord.Patient.LastName;
            row[4] = appointment.TimeSlot.Start.ToString();
            row[5] = appointment.TimeSlot.End.ToString();
            return row;
        }
        
        private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            _scheduleService.Refresh();
            _doctorService.Refresh();
            Doctor doc = _doctorService.DoctorCookie();
            List<Appointment> docList = _scheduleService.GetAppointmentsByDoctorId(doc.Id);
            if (dgAllAppointments.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select an appointment.");

            } else
            {
                var selectedAppointment = Convert.ToInt32((dgAllAppointments.SelectedItem as DataRowView).Row.ItemArray[0]);
                Appointment appointmentToUpdate = _scheduleService.GetAppointmentById(selectedAppointment);
                UpdateAppointment ua = new UpdateAppointment(appointmentToUpdate);
                ua.Show();
                this.Close();
            }
            LoadCancelAppointments(doc);
        }
        
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {            

            _scheduleService.Refresh();
            _doctorService.Refresh();
            Doctor doc = _doctorService.DoctorCookie();
            List<Appointment> docList = _scheduleService.GetAppointmentsByDoctorId(doc.Id);
            if (dgAllAppointments.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select an appointment.");

            }
            else
            {
                var selectedAppointment = Convert.ToInt32((dgAllAppointments.SelectedItem as DataRowView).Row.ItemArray[0]);
                MessageBoxResult confirm = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    _scheduleService.CancelAppointment(selectedAppointment);
                    MessageBox.Show("Appointment deleted..");
                }
                else
                {
                    Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is MessageBox)?.Close();
                }
            }
            LoadCancelAppointments(doc);
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

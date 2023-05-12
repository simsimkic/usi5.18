using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    /// <summary>
    /// Interaction logic for DoctorAllExaminations.xaml
    /// </summary>
    public partial class DoctorAllExaminations : Window
    {
        private DoctorService _doctorService = new DoctorService();
        private ScheduleService _scheduleService = new ScheduleService();
        public DoctorAllExaminations()
        {
            InitializeComponent();
            _scheduleService.Refresh();
            Doctor doctor = _doctorService.DoctorCookie();
            LoadCancelAppointments(doctor);
        }

        public void LoadCancelAppointments(Doctor doctor)
        {
            DataTable dataTable = LoadDataTable();
            List<Appointment> allAppointments = _scheduleService.GetAppointmentsForToday(_scheduleService.GetAppointmentsByDoctorId(doctor.Id));
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
            DataColumn room = new DataColumn("Room", typeof(String)); dt.Columns.Add(room);
            return dt;
        }

        public DataRow LoadDataRow(DataTable dataTable, Appointment appointment)
        {
                DataRow row = dataTable.NewRow();
                row[0] = appointment.Id;
                row[1] = appointment.MedicalRecord.Patient.Id;
                row[2] = appointment.MedicalRecord.Patient.FirstName;
                row[3] = appointment.MedicalRecord.Patient.LastName;
                row[4] = appointment.TimeSlot.Start.ToString();
                row[5] = appointment.TimeSlot.End.ToString();
                row[6] = appointment.RoomId.ToString();
                return row;
        }

        private void btnExamine_OnClick(object sender, RoutedEventArgs e)
        {
            _scheduleService.Refresh();

            if (dgAllAppointments.SelectedIndex == -1)
            {
                MessageBox.Show("Missing ID.");
            }
            else
            {
                var selectedAppointment = Convert.ToInt32((dgAllAppointments.SelectedItem as DataRowView).Row.ItemArray[0]);
                Appointment appointment = _scheduleService.GetAppointmentById(selectedAppointment);
                DoctorExamine de = new DoctorExamine(appointment);
                de.Show();
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

        private void miSearch_OnClick(object sender, RoutedEventArgs e)
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    public partial class DoctorPatientSearch : Window
    {

        private DoctorService _doctorService = new DoctorService();
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private ScheduleService _scheduleService = new ScheduleService();
        public DoctorPatientSearch()
        {
            InitializeComponent();
            LoadAllPatients();
        }

        public void LoadAllPatients()
        {
            _medicalRecordService.Refresh();
            DataTable dt = new DataTable();
            DataColumn id = new DataColumn("ID", typeof(int)); dt.Columns.Add(id);
            DataColumn firstname = new DataColumn("First Name", typeof(String)); dt.Columns.Add(firstname);
            DataColumn lastname = new DataColumn("Last Name", typeof(String)); dt.Columns.Add(lastname);
            List<MedicalRecord> records = _medicalRecordService.GetAllMedicalRecords();
            foreach (MedicalRecord mr in records)
            {
                DataRow row = dt.NewRow();
                row[0] = mr.Id;
                row[1] = mr.Patient.FirstName;
                row[2] = mr.Patient.LastName;
                dt.Rows.Add(row);
            }
            dgPatientInfo.ItemsSource = dt.DefaultView;
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

        private void btwShowPatientInfo_Click(object sender, RoutedEventArgs e)
        {


            _medicalRecordService.Refresh();
            _scheduleService.Refresh();
            List<Appointment> doctorsAppointments = _scheduleService.GetDoctorAppointments(_doctorService.DoctorCookie());

            if (dgPatientInfo.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a patient.");
            }
            else
            {
                var selectedPatient = Convert.ToInt32((dgPatientInfo.SelectedItem as DataRowView).Row.ItemArray[0]);
                if (!doctorsAppointments.Any(x => x.MedicalRecord.Id == selectedPatient))

                {
                    MessageBox.Show("This is not your patient.");
                }
                else
                {
                    DoctorUpdatePatient dup =
                        new DoctorUpdatePatient(_medicalRecordService.GetMedicalRecordById(selectedPatient));
                    dup.ShowDialog();
                }
            }
        }

        private void miOverview_OnClick(object sender, RoutedEventArgs e)
        {
            DoctorAllExaminations dae = new DoctorAllExaminations();
            dae.Show();
            this.Close();
        }
    }
}

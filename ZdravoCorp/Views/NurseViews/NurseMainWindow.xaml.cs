using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.NurseViews
{
    /// <summary>
    /// Interaction logic for NurseWindow.xaml
    /// </summary>
    public partial class NurseWindow : Window
    {
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private PatientService _patientService = new PatientService();
        public NurseWindow()
        {
            InitializeComponent();
            LoadLoggedNurse();
            PopulateMedicalRecordsDataGrid();
        }

        private void LoadLoggedNurse()
        {
            NurseRepository nr = new NurseRepository();
            Nurse loggedInAs = nr.ReadCookie();
            LabelHello.Content = "Hello " + loggedInAs.Username + ", you are logged in as NURSE !";
        }

        void PopulateMedicalRecordsDataGrid()
        {
            DataTable dt = new DataTable();
            DataColumn id = new DataColumn("ID", typeof(int)); dt.Columns.Add(id);
            DataColumn firstname = new DataColumn("First Name", typeof(String)); dt.Columns.Add(firstname);
            DataColumn lastname = new DataColumn("Last Name", typeof(String)); dt.Columns.Add(lastname);
            DataColumn username = new DataColumn("Username", typeof(String)); dt.Columns.Add(username);
            List<MedicalRecord> medicalRecords = _medicalRecordService.GetAllMedicalRecords();
            foreach (MedicalRecord medRec in medicalRecords)
            {
                DataRow row = dt.NewRow();
                row[0] = medRec.Id;
                row[1] = medRec.Patient.FirstName;
                row[2] = medRec.Patient.LastName;
                row[3] = medRec.Patient.Username;
                dt.Rows.Add(row);
            }
            PatientsDataGrid.ItemsSource = dt.DefaultView;
        }

        private MedicalRecord GetMedicalRecordFromDataGrid()
        {
            DataRowView dataRowView = (DataRowView)PatientsDataGrid.SelectedItem;
            DataRow dataRow = dataRowView.Row;
            int id = (int)dataRow["Id"];
            _medicalRecordService.Refresh();
            return _medicalRecordService.GetMedicalRecordById(id);
        }
        private void PatientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem != null)
            {
                UpdateButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                UrgentButton.IsEnabled = true;
                ReceptionButton.IsEnabled = true;
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NurseAddPacientWindow nurseAddPacientWindow = new NurseAddPacientWindow();
            nurseAddPacientWindow.Show();
            this.Hide();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

            if (PatientsDataGrid.SelectedItem != null)
            {
                List<MedicalRecord> medicalRecords = _medicalRecordService.GetAllMedicalRecords();
                MedicalRecord medicalRecord = GetMedicalRecordFromDataGrid();
                NurseUpdatePacientWindow nurseUpdatePacientWindow = new NurseUpdatePacientWindow(medicalRecord);
                nurseUpdatePacientWindow.Show();
                this.Close();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem != null)
            {
                _medicalRecordService.Refresh();
                MedicalRecord medicalRecordToDelete = GetMedicalRecordFromDataGrid();
                _medicalRecordService.DeleteMedicalRecord(medicalRecordToDelete.Id);
                _patientService.Refresh();
                _patientService.DeletePatient(medicalRecordToDelete.Patient.Id);
                PopulateMedicalRecordsDataGrid();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            clearFile();
            LoginScreen ls = new LoginScreen();
            ls.Show();
            this.Close();
        }

        public void clearFile()
        {
            string path = @"..\\..\\..\\Data\\cookie.json";
            File.WriteAllText(path, string.Empty);
        }

        private void ReceptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem != null)
            {
                MedicalRecord medicalRecord = GetMedicalRecordFromDataGrid();
                AddmissionOfPatientWindow addmissionWindow = new AddmissionOfPatientWindow(medicalRecord);
                addmissionWindow.Show();
                this.Close();
            }
        }

        private void UrgentButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem != null)
            {
                MedicalRecord medicalRecord = GetMedicalRecordFromDataGrid();
                UrgentAppointmentWindow urgentAppointmentWindow = new UrgentAppointmentWindow(medicalRecord);
                urgentAppointmentWindow.Show();
                this.Close();
            }
        }
    }
}

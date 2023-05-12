using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using static System.Net.Mime.MediaTypeNames;

namespace ZdravoCorp.Views.NurseViews
{
    /// <summary>
    /// Interaction logic for NurseUpdatePacientWindow.xaml
    /// </summary>
    public partial class NurseUpdatePacientWindow : Window
    {

        private MedicalRecord SelectedMedRecord { get; }
        private List<MedicalRecord> MedicalRecords { get; }
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private PatientService _patientService = new PatientService();
        public NurseUpdatePacientWindow(MedicalRecord selectedRowPatient)
        {
            InitializeComponent();
            this.SelectedMedRecord = selectedRowPatient;
            this.MedicalRecords = _medicalRecordService.GetAllMedicalRecords();
            LoadDataTextBox();  
        }

        void LoadDataTextBox()
        {
            UsernameTextBox.Text = SelectedMedRecord.Patient.Username;
            PasswordTextBox.Text = SelectedMedRecord.Patient.Password;
            NameTextBox.Text = SelectedMedRecord.Patient.FirstName;
            SurnameTextBox.Text = SelectedMedRecord.Patient.LastName;
            IdTextBox.Text = SelectedMedRecord.Patient.Id.ToString();
            HeightTextBox.Text = SelectedMedRecord.Height.ToString();
            WeightTextBox.Text = SelectedMedRecord.Weight.ToString();
            string stringDisease = string.Join(",", SelectedMedRecord.DiseaseHistory);
            DiseaseTextBox.Text = stringDisease;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NurseWindow nurseWindow = new NurseWindow();
            nurseWindow.Show();
            this.Close();
        }

        private bool IsIdAlreadyUsed()
        {
            foreach (MedicalRecord medRec in MedicalRecords)
            {
                if (medRec.Patient.Id.ToString() == IdTextBox.Text)
                {
                    if (medRec != SelectedMedRecord)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsUsernameAlreadyUsed()
        {
            foreach (MedicalRecord medRec in MedicalRecords)
            {
                if (medRec != SelectedMedRecord)
                {
                    if (medRec.Patient.Username == UsernameTextBox.Text)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int test = 0;
            if (IsIdAlreadyUsed())
            {
                IdErrorLabel.Content = "Already used ID !";
                test = 1;
            }
            else
            {
                IdErrorLabel.Content = "";
            }
            if (IsUsernameAlreadyUsed())
            {
                UsernameErrorLabel.Content = "Already used username";
                test = 1;
            }else {
                UsernameErrorLabel.Content = "";
            }

            if (!IsValidName(NameTextBox.Text))
            {
                NameErrorLabel.Content = "Only letters!";
                test = 1;
            }
            else
            {
                NameErrorLabel.Content = "";
            }

            if (!IsValidUsername(UsernameTextBox.Text))
            {
                UsernameErrorLabel.Content = "Only letters and numbers!";
                test = 1;
            }
            else
            {
                UsernameErrorLabel.Content = "";
            }

            if (IsUsernameAlreadyUsed(UsernameTextBox.Text))
            {
                UsernameErrorLabel.Content = "Already used username!";
                test = 1;
            }
            else
            {
                UsernameErrorLabel.Content = "";
            }

            if (!IsValidPassword(PasswordTextBox.Text))
            {
                PasswordErrorLabel.Content = "Only letters and numbers!";
                test = 1;
            }
            else
            {
                PasswordErrorLabel.Content = "";
            }

            if (!IsValidSurname(SurnameTextBox.Text))
            {
                SurnameErrorLabel.Content = "Only letters!";
                test = 1;
            }
            else
            {
                SurnameErrorLabel.Content = "";
            }

            if (!IsValidNumber(HeightTextBox.Text))
            {
                HeightErrorLabel.Content = "Numbers only!";
                test = 1;
            }
            else
            {
                HeightErrorLabel.Content = "";
            }


            if (!IsValidNumber(WeightTextBox.Text))
            {
                WeightErrorLabel.Content = "Numbers only!";
                test = 1;
            }
            else
            {
                WeightErrorLabel.Content = "";
            }
            if (test == 0)
            {
                UpdateRecordAndPatient();
                NurseWindow nurseWindow = new NurseWindow();
                nurseWindow.Show();
                this.Close();
            }
        }
        private bool IsValidName(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z]+$");
        }

        private bool IsValidUsername(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z0-9]+$");
        }

        private bool IsUsernameAlreadyUsed(string username)
        {
            foreach (MedicalRecord medRec in MedicalRecords)
            {
                if (medRec.Patient.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsValidPassword(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z0-9]+$");
        }

        private bool IsValidSurname(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z0-9]+$");
        }

        private bool IsValidNumber(string text)
        {
            return Regex.IsMatch(text, "^[0-9]+$");
        }

        private void UpdateRecordAndPatient()
        {
            int id = int.Parse(IdTextBox.Text);
            string patientUsername = UsernameTextBox.Text;
            string patientPassword = PasswordTextBox.Text;
            string patientName = NameTextBox.Text;
            string patientSurname = SurnameTextBox.Text;
            int patientHeight = int.Parse(HeightTextBox.Text);
            int patientWeight = int.Parse(WeightTextBox.Text);
            List<string> patientDiseaseList = DiseaseTextBox.Text.Split(',').ToList();
            Patient patient = new Patient(id, patientUsername, patientPassword, patientName, patientSurname);
            _medicalRecordService.UpdateMedicalRecord(id, patient, patientHeight, patientWeight, patientDiseaseList);
        }
    }
}

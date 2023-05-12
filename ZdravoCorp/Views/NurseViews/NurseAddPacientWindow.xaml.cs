using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;

namespace ZdravoCorp.Views.NurseViews
{
    /// <summary>
    /// Interaction logic for NurseAddPacientWindow.xaml
    /// </summary>
    public partial class NurseAddPacientWindow : Window
    {
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private PatientService _patientService = new PatientService();
        private List<MedicalRecord> MedicalRecords { get; }
        public NurseAddPacientWindow()
        {
            InitializeComponent();
            this.MedicalRecords = _medicalRecordService.GetAllMedicalRecords();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NurseWindow nurseWindow = new NurseWindow();
            nurseWindow.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int test = 0;
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
            }else
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
            }else
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
                AddMedicalRecordAndPatient();
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
        
        private void AddMedicalRecordAndPatient()
        {
            string patientUsername = UsernameTextBox.Text;
            string patientPassword = PasswordTextBox.Text;
            string patientName = NameTextBox.Text;
            string patientSurname = SurnameTextBox.Text;
            int patientHeight = int.Parse(HeightTextBox.Text);
            int patientWeight = int.Parse(WeightTextBox.Text);
            List<string> patientDiseaseList = new List<String>();
            patientDiseaseList = DiseaseTextBox.Text.Split(',').ToList();
            Patient patient = new Patient(_medicalRecordService.GenerateMedicalRecordId(), patientUsername, patientPassword, patientName, patientSurname);
            MedicalRecord medRec = new MedicalRecord(patient.Id, patient, patientWeight, patientHeight, patientDiseaseList);
            _medicalRecordService.AddMedicalrecord(medRec);
            _patientService.AddPatient(patient);
        }
    }
}

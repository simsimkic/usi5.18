using System;
using System.Collections.Generic;
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
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    public partial class DoctorUpdatePatient : Window
    {
        private MedicalRecord _currentRecord;
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private PatientService _patientService = new PatientService();
        public DoctorUpdatePatient(MedicalRecord mr)
        {
            InitializeComponent();
            _currentRecord = mr;
            LoadPatientData(mr);
        }

        public void LoadPatientData(MedicalRecord mr)
        {
            tbMedicalId.Text = mr.Id.ToString();
            tbFirstName.Text = mr.Patient.FirstName.ToString();
            tbLastName.Text = mr.Patient.LastName.ToString();
            tbPatientId.Text = mr.Patient.Id.ToString();
            tbWeight.Text = mr.Weight.ToString();
            tbHeight.Text = mr.Height.ToString();
            tbDiseaseHistory.Text = string.Join(',', mr.DiseaseHistory.ToArray());
            foreach (String el in mr.DiseaseHistory) { lbDiseases.Items.Add(el); }
        }

        private void btnDeleateDisease_Click(object sender, RoutedEventArgs e)
        {
            if (lbDiseases.SelectedIndex == -1 )
            {
                MessageBox.Show("You need to select a disease.");
            }
            else
            {
                lbDiseases.Items.Remove(lbDiseases.SelectedIndex);
                _currentRecord.DiseaseHistory.RemoveAt(_currentRecord.DiseaseHistory.IndexOf(lbDiseases.SelectedItem.ToString()));
                lbDiseases.Items.Clear();
                foreach (String el in _currentRecord.DiseaseHistory.ToArray()) { lbDiseases.Items.Add(el); }
            }
        }
        
        private void btnAddDisease_Click(object sender, RoutedEventArgs e)
        {
            if (tbDiseaseToList.Text == "")
            {
                MessageBox.Show("You didn't enter any diseases.");
            }
            else
            {
                lbDiseases.Items.Add(tbDiseaseToList.Text);
                _currentRecord.DiseaseHistory.Add(tbDiseaseToList.Text);
                tbDiseaseToList.Clear();
            }
        }
        
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!Double.TryParse(tbWeight.Text, out _) || !Double.TryParse(tbHeight.Text, out _))
            {
                MessageBox.Show("Invalid input.");
            }
            else
            {
                _medicalRecordService.Refresh();
                _patientService.Refresh();
                _medicalRecordService.UpdateMedicalRecord(Convert.ToInt32(tbMedicalId.Text),
                                                            _patientService.GetPatientById(Convert.ToInt32(tbPatientId.Text))
                                                        , Convert.ToDouble(tbHeight.Text),
                                                        Convert.ToDouble(tbWeight.Text),
                                                                _currentRecord.DiseaseHistory);
                MessageBox.Show("Patient updated.");
            }
        }

        private void miLogout_Click(object sender, RoutedEventArgs e)
        {
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
        
        private void btnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

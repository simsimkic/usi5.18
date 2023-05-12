using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.DoctorViews
{
    /// <summary>
    /// Interaction logic for PatientInfo.xaml
    /// </summary>
    public partial class PatientInfo : Window
    {
        public PatientInfo(MedicalRecord mr)
        {
            InitializeComponent();
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

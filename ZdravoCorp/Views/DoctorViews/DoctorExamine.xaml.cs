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
    /// <summary>
    /// Interaction logic for DoctorExamine.xaml
    /// </summary>
    /// 
    public partial class DoctorExamine : Window
    {
        private Appointment _currentAppointment;
        private MedicalRecordService _medicalRecordService = new MedicalRecordService();
        private ExaminationService _examinationService = new ExaminationService();
        private ScheduleService _scheduleService = new ScheduleService();
        public DoctorExamine(Appointment appointment)
        {
            InitializeComponent();
            _currentAppointment = appointment;
            tbMedicalId.Text = _currentAppointment.MedicalRecord.Id.ToString();
        }
        
        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            _currentAppointment.Status = Appointment.AppointmentStatus.Compleated;
            _scheduleService.Refresh();
            _examinationService.Refresh();
            _examinationService.AddExamination(new Examination(_examinationService.GenerateId(), _currentAppointment.MedicalRecord.Id, _currentAppointment.TimeSlot, tbAnamnesis.Text));
            _scheduleService.UpdateAppointment(_currentAppointment.Id, _currentAppointment.TimeSlot, _currentAppointment.Doctor, _currentAppointment.MedicalRecord, Appointment.AppointmentStatus.Compleated, _currentAppointment.Type, _currentAppointment.RoomId);
            DoctorDynamicEquipment dde  = new DoctorDynamicEquipment(_currentAppointment.RoomId);
            dde.ShowDialog();
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
        private void miSearch_Click(object sender, RoutedEventArgs e)
        {
            DoctorPatientSearch dps = new DoctorPatientSearch();
            dps.Show();
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            _medicalRecordService.Refresh();
            DoctorUpdatePatient dup = new DoctorUpdatePatient(_medicalRecordService.GetMedicalRecordById(_currentAppointment.MedicalRecord.Id));
            dup.ShowDialog();
        }
        private void miOverview_OnClick(object sender, RoutedEventArgs e)
        {
            DoctorAllExaminations dae = new DoctorAllExaminations();
            dae.Show();
            this.Close();
        }
    }
}

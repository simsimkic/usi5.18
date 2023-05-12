using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.DoctorViews;
using ZdravoCorp.Views.ManagerViews;
using ZdravoCorp.Views.NurseViews;

namespace ZdravoCorp.Views.LoginViews
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private DoctorService _doctorService = new DoctorService();
        public LoginScreen()
        {
            InitializeComponent();
            ClearFile();
        }

        public void ClearFile()
        {
            string path = @"..\\..\\..\\Data\\cookie.json";
            File.WriteAllText(path, string.Empty);
        }

        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            tbPassword.Clear();
            tbUsername.Clear();
        }

        private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();

            Dictionary<String, String> loginMap = new Dictionary<string, string>();
            GetLoginInfo(loginMap);
            if (username == "" || password == "")
            {
                MessageBox.Show("All fields required.");
            }
            else
            {
                if (!loginMap.ContainsKey(username))
                {
                    MessageBox.Show("User not found");
                }
                else if (loginMap[username].Split("|")[0] != password)
                {
                    MessageBox.Show("Password incorrect.");
                }
                else
                {
                    string userType = loginMap[username].Split('|')[1];

                    switch (userType)
                    {
                        case "doctor":
                            _doctorService.Refresh();
                            _doctorService.DoctorLogin(_doctorService.GetDoctorByUsername(username));
                            DoctorLandingRedirect();
                            break;
                        
                        case "patient":
                            break;
                            
                        case "manager":
                            ManagerRepository mr = new ManagerRepository();
                            ManagerLandingPage mlp = new ManagerLandingPage();
                            mr.LoadManagersFromJson();
                            Manager managerLogin = mr.FindManagerByUsername(username);
                            mr.LoggedManager(managerLogin);
                            mlp.Show();
                            this.Close();
                            break;
                            
                        case "nurse":
                            NurseRepository nr = new NurseRepository(); nr.LoadNursesFromJson();
                            Nurse nurseLogin = nr.FindNurseByUsername(username);
                            nr.LoggedNurse(nurseLogin);
                            NurseWindow nmw = new NurseWindow();
                            nmw.Show();
                            this.Close();
                            break;
                    }
                }
            }

        }
        
        public void GetLoginInfo(Dictionary<String, String> loginMap)
        {
            _doctorService.Refresh();
            _doctorService.LoadDoctorLogin(loginMap);
            
            NurseRepository nr = new NurseRepository();
            nr.LoadNursesFromJson();
            nr.LoadNurseLogin(loginMap);

            
            ManagerRepository mr = new ManagerRepository();
            mr.LoadManagersFromJson();
            mr.LoadManagerLogin(loginMap);
            
            //append user info to login map for patient and nurse

        }
        
        public void DoctorLandingRedirect()
        {
                DoctorLandingPage dlp = new DoctorLandingPage();
                dlp.Show();
                this.Close();
        }
    }
}

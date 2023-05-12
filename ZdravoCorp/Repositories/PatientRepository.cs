using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;

namespace ZdravoCorp.Repositories
{
    public class PatientRepository
    {
        private List<Patient> Patients = new List<Patient> { };
        public String path = @"..\\..\\..\\Data\\patients.json";

        public void LoadPatientsFromJson()
        {
            string jsonData = File.ReadAllText(this.path);
            Patients = JsonSerializer.Deserialize<List<Patient>>(jsonData);
        }

        public List<Patient> GetAllPatients()
        {
            LoadPatientsFromJson();
            return Patients;
        }

        public bool PatientExists(int id)
        {
            if (Patients.Any(x => x.Id == id)) return true;
            return false;
        }

        public Patient GetPatientById(int id)
        {
            if (PatientExists(id))
                return Patients.Find(x => x.Id == id);
            return null;
        }

        public void WriteToJson()
        {
            string jsonData = JsonSerializer.Serialize(Patients);
            File.WriteAllText(path, jsonData);
        }

        public void AddPatient(Patient Patient)
        {
            if (!PatientExists(Patient.Id))
            {
                Patients.Add(Patient);
                WriteToJson();
            }
        }
        public void LoadPatientLogin(Dictionary<String, String> loginMap)
        {
            foreach (Patient pat in Patients) { loginMap.Add(pat.Username, pat.Password + "|" + "patient"); }
        }

        public Patient FindPatientByUsername(String username)
        {
            return Patients.Find(x => x.Username == username);
        }

        public void LoggedPatient(Patient patient)
        {
            string jsonData = JsonSerializer.Serialize(patient);
            File.WriteAllText(@"..\\..\\..\\Data\\cookie.json", jsonData);
        }
        public void DeletePatient(int id)
        {
            if (PatientExists(id))
            {
                Patient patientToRemove = GetPatientById(id);
                Patients.Remove(patientToRemove);
                WriteToJson();
            }
        }
        public Patient ReadCookie()
        {
            string jsonData = File.ReadAllText(@"..\\..\\..\\Data\\cookie.json");
            return JsonSerializer.Deserialize<Patient>(jsonData);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Domain.UserClasses;

namespace ZdravoCorp.Repositories
{
    public class DoctorRepository
    {
        private List<Doctor> _doctors;
        public String path = @"..\\..\\..\\Data\\doctors.json";

        public void LoadDoctorsFromJson()
        {
            string jsonData = File.ReadAllText(this.path);
            _doctors = JsonSerializer.Deserialize<List<Doctor>>(jsonData);
        }

        public List<Doctor> GetAllDoctors()
        {
            LoadDoctorsFromJson();
            return _doctors;
        }

        public bool DoctorExists(int id)
        {
            return _doctors.Any(x => x.Id == id);
        }

        public Doctor GetDoctorById(int id)
        {
            if (DoctorExists(id))
                return _doctors.Find(x => x.Id == id);
            return null;
        }

        public void WriteToJson()
        {
            string jsonData = JsonSerializer.Serialize(_doctors);
            File.WriteAllText(path, jsonData);
        }

        public void AddDoctor(Doctor doctor)
        {
            if (!DoctorExists(doctor.Id))
            {
                _doctors.Add(doctor);
                WriteToJson();
            }
        }
        public void DeleteDoctor(int id)
        {
            if (DoctorExists(id))
            {
                Doctor doctorToRemove = GetDoctorById(id);
                _doctors.Remove(doctorToRemove);
                WriteToJson();
            }
        }

        public void LoadDoctorLogin(Dictionary<String, String> loginMap)
        {
            foreach(Doctor doc in _doctors) {loginMap.Add(doc.Username, doc.Password + "|" + doc.Role); }
        }

        public Doctor GetDoctorByUsername(String username)
        {
            return _doctors.Find(x => x.Username == username);
        }

        public void DoctorLogin(Doctor doctor)
        {
            string jsonData = JsonSerializer.Serialize(doctor);
            File.WriteAllText(@"..\\..\\..\\Data\\cookie.json", jsonData);
        }

        public Doctor DoctorCookie()
        {
            string jsonData = File.ReadAllText(@"..\\..\\..\\Data\\cookie.json");
            return JsonSerializer.Deserialize<Doctor>(jsonData);
        }
    }
}

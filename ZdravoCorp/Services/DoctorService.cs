using System;
using System.Collections.Generic;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    public class DoctorService
    {
        private DoctorRepository _doctorRepository;

        public DoctorService()
        {
            _doctorRepository = new DoctorRepository();
            _doctorRepository.LoadDoctorsFromJson();
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAllDoctors();
        }

        public Doctor GetDoctorById(int id)
        {
            return _doctorRepository.GetDoctorById(id);
        }

        public Doctor GetDoctorByUsername(String username)
        {
            return _doctorRepository.GetDoctorByUsername(username);
        }

        public void AddDoctor(Doctor doctor)
        {
            _doctorRepository.AddDoctor(doctor);
        }

        public void DeleteDoctor(int id)
        {
            _doctorRepository.DeleteDoctor(id);
        }

        public void LoadDoctorLogin(Dictionary<String, String> loginMap)
        {
            _doctorRepository.LoadDoctorLogin(loginMap);
        }

        public Doctor DoctorCookie()
        {
            return _doctorRepository.DoctorCookie();
        }

        public void DoctorLogin(Doctor doctor)
        {
            _doctorRepository.DoctorLogin(doctor);
        }

        public void Refresh()
        {
            _doctorRepository.LoadDoctorsFromJson();
        }
    }
}

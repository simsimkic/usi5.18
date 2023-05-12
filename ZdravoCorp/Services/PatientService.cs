using System.Collections.Generic;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services;

public class PatientService
{
    private PatientRepository _patientRepository;

    public PatientService()
    {
        _patientRepository = new PatientRepository();
        _patientRepository.LoadPatientsFromJson();
    }

    public List<Patient> GetAllPatients()
    {
        return _patientRepository.GetAllPatients();
    }
    public Patient GetPatientById(int id)
    {
        return _patientRepository.GetPatientById(id);
    }

    public void AddPatient(Patient patient)
    {
        _patientRepository.AddPatient(patient);
    }

    public void DeletePatient(int id)
    {
        _patientRepository.DeletePatient(id);
    }

    public void Refresh()
    {
        _patientRepository.LoadPatientsFromJson();
    }
}
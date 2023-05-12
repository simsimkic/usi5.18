using System;
using System.Collections.Generic;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    public class MedicalRecordService
    {
        private MedicalRecordRepository _repository;

        public MedicalRecordService()
        {
            _repository = new MedicalRecordRepository();
            _repository.LoadMedicalRecordFromJson();
        }

        public List<MedicalRecord> GetAllMedicalRecords()
        {
            return _repository.GetAllMedicalRecords();
        }


        public int GenerateMedicalRecordId()
        {
            return _repository.GenerateMedicalRecordId();
        }

        public MedicalRecord GetMedicalRecordById(int id)
        {
            return _repository.GetMedicalRecordById(id);
        }

        public MedicalRecord GetMedicalRecordByPatientId(int id)
        {
            return _repository.FindByPatientId(id);
        }

        public void AddMedicalrecord(MedicalRecord medicalRecord)
        {
            _repository.AddMedicalRecord(medicalRecord);
        }

        public void DeleteMedicalRecord(int id)
        {
            _repository.DeleteMedicalRecord(id);
        }

        public void UpdateMedicalRecord(int id, Patient patient, double height, double weight, List<String> diseaseHistory)
        {
            _repository.UpdateMedicalRecord(id, patient, height, weight, diseaseHistory);
        }
        
        public void Refresh()
        {
            _repository.LoadMedicalRecordFromJson();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;

namespace ZdravoCorp.Repositories
{
    public class MedicalRecordRepository
    {
        private List<MedicalRecord> medRec;
        public String path = @"..\\..\\..\\Data\\medicalrecord.json";

        public void LoadMedicalRecordFromJson()
        {
            string jsonData = File.ReadAllText(path);
            medRec = JsonSerializer.Deserialize<List<MedicalRecord>>(jsonData);
        }

        public List<MedicalRecord> GetAllMedicalRecords()
        {
            LoadMedicalRecordFromJson();
            return medRec;
        }

        public int GenerateMedicalRecordId()
        {
            int id = 0;
            while (true)
            {
                bool randomTest = true;
                Random rnd = new Random();
                id = rnd.Next(1, 1000);
                foreach (MedicalRecord mR in medRec)
                {
                    if (mR.Id == id)
                    {
                        randomTest = false;
                        break;
                    }
                }
                if (randomTest)
                {
                    return id;
                }
            }
        }

        public bool MedicalRecordExists(int id)
        {
            return medRec.Any(x => x.Id == id);
        }

        public MedicalRecord GetMedicalRecordById(int id)
        {
            if (MedicalRecordExists(id))
            {
                return medRec.Find(x => x.Id == id);
            }
            return null;
        }

        public void WriteToJson()
        {
            string jsonData = JsonSerializer.Serialize(medRec);
            File.WriteAllText(path, jsonData);
        }

        public MedicalRecord FindByPatientId(int id)
        {
            foreach (MedicalRecord med in medRec)
            {
                if (med.Patient.Id == id)
                    return med;
            }
            return null;
        }

        public void DeleteMedicalRecord(int id)
        {
            if (MedicalRecordExists(id))
            {
                MedicalRecord medicalRecordToRemove = GetMedicalRecordById(id);
                medRec.Remove(medicalRecordToRemove);
                WriteToJson();
            }
        }

        public void AddMedicalRecord(MedicalRecord medicalRecord)
        {
            if (!MedicalRecordExists(medicalRecord.Id))
            {
                medRec.Add(medicalRecord);
                WriteToJson();
            }
        }

        public void UpdateMedicalRecord(int id, Patient patient, double height, double weight, List<String> diseaseHistory)
        {
            MedicalRecord medicalRecordtoUpdate = GetMedicalRecordById(id);
            medicalRecordtoUpdate.Patient = patient;
            medicalRecordtoUpdate.Height = height;
            medicalRecordtoUpdate.Weight = weight;
            medicalRecordtoUpdate.DiseaseHistory = diseaseHistory;
            WriteToJson();
        }
    }
}

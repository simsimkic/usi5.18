using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Domain.UserClasses;

namespace ZdravoCorp.Domain
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<string> DiseaseHistory { get; set; }

        public MedicalRecord(int id, Patient patient, double height, double weight, List<string> diseaseHistory)
        {
            Id = id;
            this.Patient = patient;
            Height = height;
            Weight = weight;
            DiseaseHistory = diseaseHistory;
        }
    }
}

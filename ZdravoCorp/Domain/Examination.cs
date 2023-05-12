using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Domain
{
    public class Examination
    {
        public int Id { get; set; }
        public int MedicalRecordId { get; set; }
        public TimeSlot Date { get; set; }
        public String Report { get; set; }

        public Examination(int id, int medicalRecordId, TimeSlot date, String report)
        {
            this.Id = id;
            this.MedicalRecordId = medicalRecordId;
            this.Date = date;
            this.Report = report;
        }
    }
}

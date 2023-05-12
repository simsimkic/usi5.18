using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class ExaminationRepository
    {
        private List<Examination> _examinations;
        public String path = "..\\..\\..\\Data\\examinations.json";

        public void LoadExaminationsFromJson()
        {
            _examinations = JsonSerializer.Deserialize<List<Examination>>(File.ReadAllText(this.path));
        }

        public void WriteToJson()
        {
            File.WriteAllText(this.path, JsonSerializer.Serialize(_examinations));
        }

        public List<Examination> GetAllExaminations()
        {
            LoadExaminationsFromJson();
            return _examinations;
        } 

        public void AddExamination(Examination examination)
        {
            _examinations.Add(examination);
            WriteToJson();
        }
        

        public bool ExaminationExist(int id)
        {
            return _examinations.Any(x => x.Id == id);
        }

        public Examination GetExaminationById(int id)
        {
            return _examinations.Find(x => x.Id == id);
        }

        public int GenerateId()
        {
            while (true)
            {
                int id = new Random().Next(100000);
                if (!ExaminationExist(id))
                    return id;
            }
        }

    }
}

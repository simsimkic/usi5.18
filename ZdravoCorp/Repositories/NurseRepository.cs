using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Domain.UserClasses;

namespace ZdravoCorp.Repositories
{
    public class NurseRepository
    {
        private List<Nurse> nurses;
        public String path = @"..\\..\\..\\Data\\nurses.json";

        public void LoadNursesFromJson()
        {
            string jsonData = File.ReadAllText(this.path);
            nurses = JsonSerializer.Deserialize<List<Nurse>>(jsonData);
        }

        public List<Nurse> GetAllNurses()
        {
            LoadNursesFromJson();
            return nurses;
        }

        public bool NurseExists(int id)
        {
            return nurses.Any(x => x.Id == id);
        }

        public Nurse GetNurseById(int id)
        {
            if (NurseExists(id))
                return nurses.Find(x => x.Id == id);
            return null;
        }

        public void WriteToJson()
        {
            string jsonData = JsonSerializer.Serialize(nurses);
            File.WriteAllText(path, jsonData);
        }

        public void AddNurse(Nurse nurse)
        {
            if (!NurseExists(nurse.Id))
            {
                nurses.Add(nurse);
                WriteToJson();
            }
        }

        public void UpdateNurse(int id, string firstName, string lastName, string username, string password)
        {
            if (NurseExists(id))
            {
                Nurse nurseToUpdate = GetNurseById(id);
                nurseToUpdate.Username = username;
                nurseToUpdate.Password = password;
                nurseToUpdate.FirstName = firstName;
                nurseToUpdate.LastName = lastName;

                WriteToJson();
            }

        }
        public void DeleteNurse(int id)
        {
            if (NurseExists(id))
            {
                Nurse nurseToRemove = GetNurseById(id);
                nurses.Remove(nurseToRemove);
                WriteToJson();
            }
        }

        public void LoadNurseLogin(Dictionary<String, String> loginMap)
        {
            foreach (Nurse nur in nurses) { loginMap.Add(nur.Username, nur.Password + "|" + nur.Role); }
        }

        public Nurse FindNurseByUsername(String username)
        {
            return nurses.Find(x => x.Username == username);
        }

        public void LoggedNurse(Nurse nurse)
        {
            string jsonData = JsonSerializer.Serialize(nurse);
            File.WriteAllText(@"..\\..\\..\\Data\\cookie.json", jsonData);
        }

        public Nurse ReadCookie()
        {
            string jsonData = File.ReadAllText(@"..\\..\\..\\Data\\cookie.json");
            return JsonSerializer.Deserialize<Nurse>(jsonData);
        }
    }
}


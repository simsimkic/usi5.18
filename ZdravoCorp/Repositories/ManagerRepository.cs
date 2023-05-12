using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Domain.UserClasses;

namespace ZdravoCorp.Repositories
{
    public class ManagerRepository
    {
        private List<Manager> _managers;
        private const string Path = @"..\\..\\..\\Data\\managers.json";

        public void LoadManagersFromJson()
        {
            string jsonData = File.ReadAllText(Path);
            _managers = JsonSerializer.Deserialize<List<Manager>>(jsonData);
        }

        public List<Manager> GetAllManagers()
        {
            LoadManagersFromJson();
            return _managers;
        }

        private bool ManagerExists(int id)
        {
            return _managers.Any(x => x.Id == id);
        }

        private Manager GetManagerById(int id)
        {
            if (ManagerExists(id))
                return _managers.Find(x => x.Id == id);
            return null;
        }

        private void WriteToJson()
        {
            string jsonData = JsonSerializer.Serialize(_managers);
            File.WriteAllText(Path, jsonData);
        }

        public void AddManager(Manager manager)
        {
            if (!ManagerExists(manager.Id))
            {
                _managers.Add(manager);
                WriteToJson();
            }
        }

        public void UpdateManager(int id, string username, string password, string firstName, string lastName, string specialty)
        {
            if (ManagerExists(id))
            {
                Manager managerToUpdate = GetManagerById(id);
                managerToUpdate.Username = username;
                managerToUpdate.Password = password;
                managerToUpdate.FirstName = firstName;
                managerToUpdate.LastName = lastName;
                WriteToJson();
            }

        }
        public void DeleteManager(int id)
        {
            if (ManagerExists(id))
            {
                Manager managerToRemove = GetManagerById(id);
                _managers.Remove(managerToRemove);
                WriteToJson();
            }
        }

        public void LoadManagerLogin(Dictionary<String, String> loginMap)
        {
            foreach(Manager mgr in _managers) {loginMap.Add(mgr.Username, mgr.Password + "|" + mgr.Role); }
        }

        public Manager FindManagerByUsername(String username)
        {
            return _managers.Find(x => x.Username == username);
        }

        public void LoggedManager(Manager manager)
        {
            string jsonData = JsonSerializer.Serialize(manager);
            File.WriteAllText( @"..\\..\\..\\Data\\cookie.json", jsonData);
        }

        public Manager ReadCookie()
        {
            string jsonData = File.ReadAllText(@"..\\..\\..\\Data\\cookie.json");
            return JsonSerializer.Deserialize<Manager>(jsonData);
        }
    }
}

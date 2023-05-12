using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Services;

namespace ZdravoCorp.Repositories
{
    public class AppointmentRespository
    {
        private List<Appointment> _appointments;
        public String path = @"..\\..\\..\\Data\\appointments.json";

        public void LoadAppointmentsFromJson()
        {
            _appointments = JsonSerializer.Deserialize<List<Appointment>>(File.ReadAllText(this.path));
        }

        public void WriteToJson()
        {
            File.WriteAllText(this.path, JsonSerializer.Serialize(_appointments));
        }

        public List<int> GetAppointmentIds()
        {
            List<int> idList = new List<int>();
            LoadAppointmentsFromJson();
            foreach (Appointment ap in _appointments) { idList.Add(ap.Id); }
            return idList;
        }
        public int GenerateID()
        {
            List<int> allIds = GetAppointmentIds();
            int randomID;
            do
            {
                randomID = new Random().Next(1, 10001);
            } while (allIds.Contains(randomID));

            return randomID;
        }
        public int AssignRoom(RoomType type)
        {
            LoadAppointmentsFromJson();
            if (type == RoomType.ExaminationRoom)
            {
                RoomService rs = new RoomService();rs.Refresh();
                List<Room> allRooms = rs.GetRoomByType(RoomType.ExaminationRoom);
                return allRooms[new Random().Next(allRooms.Count)].Id;
            }
            else
            {
                RoomService rs = new RoomService();rs.Refresh();
                List<Room> allRooms = rs.GetRoomByType(RoomType.OperatingRoom);
                return allRooms[new Random().Next(allRooms.Count)].Id;
            }
        }

        public List<Appointment> GetALlAppointments()
        {
            return JsonSerializer.Deserialize<List<Appointment>>(File.ReadAllText(this.path));
        }

        public List<Appointment> GetDoctorAppointments(Doctor doc)
        {
            return _appointments.Where(x => x.Doctor.Id == doc.Id).ToList();
        }

        public bool AppointmentExists(int id)
        {
            return _appointments.Any(x => x.Id == id);
        }
    }
}

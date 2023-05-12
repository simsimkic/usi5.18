using System.Collections.Generic;
using System.Linq;

namespace ZdravoCorp.Domain
{
    public enum RoomType
    {
        OperatingRoom,
        ExaminationRoom,
        PatientRoom,
        WaitingRoom,
        Storage
    }

    public class Room
    {
        public int Id { get; set; }
        public RoomType Type { get; set; }
        public List<Equipment> EquipmentList { get; set; }
        public bool Free { get; set; }

        public Room(int id, RoomType type, bool free = true)
        {
            Id = id;
            Type = type;
            EquipmentList = new List<Equipment>();
            Free = free;
        }

        public void AddEquipment(Equipment equipment)
        {
            EquipmentList.Add(equipment);
        }

        public void RemoveEquipment(Equipment equipment)
        {
            EquipmentList.Remove(equipment);
        }

        public override string ToString()
        {
            List<string> equipmentStrings = new List<string>();
            
            var equipmentCounts = EquipmentList
                .GroupBy(e => e.Name)                       
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );
            
            
            foreach (var equipment in equipmentCounts)
            {
                equipmentStrings.Add(equipment.Key + ": " + equipment.Value);
            }

            string equipmentString = string.Join(", ", equipmentStrings);
            
            return "{Id: " + Id + ", " +
                   "Type: " + Type + ", " +
                   "EquipmentList: {" + equipmentString + "}}";
        }
    }
}

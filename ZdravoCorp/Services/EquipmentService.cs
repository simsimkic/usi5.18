using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    public class EquipmentService
    {
        private EquipmentRepository _equipmentRepository;

        public EquipmentService()
        {
            _equipmentRepository = new EquipmentRepository();
            _equipmentRepository.LoadEquipment();
        }

        public List<Equipment> GetAllEquipment()
        {
            return _equipmentRepository.GetAllEquipment();
        }

        public Equipment GetEquipmentById(int id)
        {
            return _equipmentRepository.GetEquipmentById(id);
        }

        public bool EquipmentExists(int id)
        {
            return _equipmentRepository.EquipmentExists(id);
        }

        public void AddEquipment(Equipment equipment)
        {
            _equipmentRepository.AddEquipment(equipment);
        }

        public void DeleteEquipment(int id)
        {
            _equipmentRepository.DeleteEquipment(id);
        }

        public void UpdateEquipmentLocationById(int id, Room destinationRoom)
        {
            _equipmentRepository.UpdateEquipmentLocationById(id, destinationRoom);
        }

        public void Refresh()
        {
            _equipmentRepository.LoadEquipment();
        }

        public bool TransferRequestExists(int equipmentId)
        {
            return _equipmentRepository.TransferRequestExists(equipmentId);
        }

        public void CreateTransferRequest(int donorId, int targetId, int equipmentId, DateTime transferTime)
        {
            _equipmentRepository.WriteTransferToFile(donorId, targetId, equipmentId, transferTime);
        }

        public void CreateEquipmentRequest(EquipmentName equipmentName, int quantity)
        {
            _equipmentRepository.WriteRequestToFile(equipmentName, quantity);
        }
        public List<Equipment> FilterEquipment(RoomType? roomTypeFilter = null, EquipmentType? equipmentTypeFilter = null, int? minQuantity = null, int? maxQuantity = null, bool excludeStorage = false)
        {
            var equipmentList = GetAllEquipment();
            var roomService = new RoomService();
            roomService.Refresh();
            
            // Create a dictionary to store the counts of equipment based on their names.
            var equipmentCounts = equipmentList.GroupBy(e => e.Name)
                .ToDictionary(group => group.Key, group => group.Count());
            
            var filteredEquipment = GetAllEquipment().Where(equipment =>
                (!roomTypeFilter.HasValue || roomService.GetRoomById(equipment.LocationId).Type == roomTypeFilter.Value) &&
                (!equipmentTypeFilter.HasValue || equipment.Type == equipmentTypeFilter.Value) &&
                (!minQuantity.HasValue || equipmentCounts[equipment.Name] >= minQuantity.Value) &&
                (!maxQuantity.HasValue || equipmentCounts[equipment.Name] <= maxQuantity.Value) &&
                (!excludeStorage || roomService.GetRoomById(equipment.LocationId).Type != RoomType.Storage)
            ).ToList();
            
            return filteredEquipment;
        }
        
        public List<Equipment> SearchEquipment(string searchTerm, List<Equipment>? equipmentList)
        {
            var roomService = new RoomService();
            roomService.Refresh();
            
            // if list not specified assume all equipment
            if (equipmentList == null)
            { 
                equipmentList = GetAllEquipment();
            }
            
            // NOTE: subsequent searches use the previously resulting reduced set of items
            return equipmentList.Where(equipment =>
                equipment.Name.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                roomService.GetRoomById(equipment.LocationId).Type.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                equipment.Type.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
        
        public Dictionary<EquipmentName, int> GetLowStockEquipmentTotal()
        {
            var equipmentList = GetAllEquipment();

            // Initialize the dictionary with all DynamicEquipment.EquipmentName enum values and set their counts to 0.
            var lowStock = Equipment.GetDynamicNames()
                .ToDictionary(name => name, name => 0);

            // Calculate the counts of dynamic equipment with the same names.
            var equipmentCounts = equipmentList
                .Where(e => e.Type == EquipmentType.Dynamic)
                .GroupBy(e => e.Name)                       
                .ToDictionary(                       
                    group => group.Key,
                    group => group.Count()
                );

            // Update the lowStock dictionary with the actual counts.
            foreach (var equipmentCount in equipmentCounts)
            {
                lowStock[equipmentCount.Key] = equipmentCount.Value;
            }

            // Filter the equipment with a count less than 5.
            var filteredLowStock = lowStock
                .Where(e => e.Value < 5)
                .ToDictionary(e => e.Key, e => e.Value);
            
            return filteredLowStock;
        }
        
        public Dictionary<Room, Dictionary<EquipmentName, int>> GetLowStockEquipmentPerRoom()
        {
            var equipmentList = GetAllEquipment();
            
            var roomService = new RoomService();
            roomService.Refresh();
            var rooms = roomService.GetAllRooms();
            
            // Dictionary for rooms
            var dynamicEquipmentPerRoom = new Dictionary<Room, Dictionary<EquipmentName, int>>();
            
            // Count equipment for each room
            foreach (Room room in rooms)
            {
                var equipmentCounts = equipmentList
                    .Where(e => roomService.GetRoomById(e.LocationId) == room)
                    .Where(e => e.Type == EquipmentType.Dynamic)
                    .GroupBy(e => e.Name)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count()
                    );

                dynamicEquipmentPerRoom[room] = equipmentCounts;
            }

            // Create Dict<Room, Dict<name, 0>>
            var filteredEquipmentDict = new Dictionary<Room, Dictionary<EquipmentName, int>>();
            var names = Equipment.GetDynamicNames();
            foreach (var room in rooms)
            {
                var equipmentDict = new Dictionary<EquipmentName, int>();
                foreach (EquipmentName name in names)
                {
                    equipmentDict[name] = 0;
                }
                filteredEquipmentDict[room] = equipmentDict;
            }
            
            foreach (var roomPair in dynamicEquipmentPerRoom)
            {
                foreach (var eqPair in roomPair.Value)
                {
                    // if count < 5 change the value from 0 to the value
                    if (eqPair.Value < 5)
                    {
                        filteredEquipmentDict[roomPair.Key][eqPair.Key] = eqPair.Value;
                    // if count >= 5 remove pair
                    } else
                    {
                        filteredEquipmentDict[roomPair.Key].Remove(eqPair.Key);
                    }
                }
            }
            return filteredEquipmentDict;
        }
    }
}

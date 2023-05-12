using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZdravoCorp.Domain;
using ZdravoCorp.Services;

namespace ZdravoCorp.Repositories
{
    public class EquipmentRepository {
        private List<Equipment> _equipment { get; set; }
        private const string EquipmentFilePath = @"..\\..\\..\\Data\\equipment.json";
        private const string RequestFilePath = @"..\\..\\..\\Data\\dynamicEquipmentRequests.json";
        private const string TransferFilePath = @"..\\..\\..\\Data\\equipmentTransferRequests.json";
        private int LastId { get; set; }
        
        public EquipmentRepository()
        {
            _equipment = new List<Equipment>();
        }

        public void LoadEquipment()
        {
            _loadEquipmentFromJson();
            ProcessEquipmentRequests();
            ProcessEquipmentTransfers();
        }

        private int _getHighestEquipmentId()
        {
            if (_equipment.Count == 0)
            {
                return 0;
            }

            return _equipment.Max(e => e.Id);
        }
        
        public void SetEquipment(List<Equipment> data)
        {
            _equipment = data;
        }
        
        private void _loadEquipmentFromJson()
        {
            LastId = _getHighestEquipmentId();
            
            // serializes the enum properties as strings
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
            
            string jsonData = File.ReadAllText(EquipmentFilePath);
            _equipment = JsonSerializer.Deserialize<List<Equipment>>(jsonData, options);
        }

        private void _writeToJson()
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
            
            string jsonData = JsonSerializer.Serialize(_equipment, options);
            File.WriteAllText(EquipmentFilePath, jsonData);
        }

        public List<Equipment> GetAllEquipment()
        {
            _loadEquipmentFromJson();
            return _equipment;
        }

        public bool EquipmentExists(int id)
        {
            return _equipment.Any(x => x.Id == id);
        }

        public Equipment GetEquipmentById(int id)
        {
            if (EquipmentExists(id))
                return _equipment.Find(x => x.Id == id);
            return null;
        }

        public void UpdateEquipmentLocationById(int id, Room destinationRoom)
        {
            if (EquipmentExists(id))
            {
                Equipment equipmentToUpdate = GetEquipmentById(id);
                equipmentToUpdate.LocationId = destinationRoom.Id;
                _writeToJson();
            }
        }
        
        public void AddEquipment(Equipment newEquipment)
        {
            if (!EquipmentExists(newEquipment.Id))
            {
                _equipment.Add(newEquipment);
                _writeToJson();
            }
        }
 
        public void DeleteEquipment(int id)
        {
            if (EquipmentExists(id))
            {
                Equipment equipmentToRemove = GetEquipmentById(id);
                _equipment.Remove(equipmentToRemove);
                _writeToJson();
            }
        }
        
        public void WriteRequestToFile(EquipmentName equipmentName, int quantity)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(RequestFilePath));

            List<EquipmentRequest> requests = new List<EquipmentRequest>();

            if (File.Exists(RequestFilePath))
            {
                string jsonData = File.ReadAllText(RequestFilePath);
                requests = JsonSerializer.Deserialize<List<EquipmentRequest>>(jsonData);
            }

            var newRequest = new EquipmentRequest(
                equipmentName.ToString(),
                quantity,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            );

            requests.Add(newRequest);

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            string jsonString = JsonSerializer.Serialize(requests, options);
            File.WriteAllText(RequestFilePath, jsonString);
        }
        
        private void ProcessEquipmentRequests()
        {
            if (!File.Exists(RequestFilePath))
            {
                Console.WriteLine("No requests found.");
                return;
            }

            string jsonData = File.ReadAllText(RequestFilePath);
            var requests = JsonSerializer.Deserialize<List<EquipmentRequest>>(jsonData);

            List<EquipmentRequest> newRequests = new List<EquipmentRequest>();

            foreach (var request in requests)
            {
                var equipmentNameString = request.Name;
                Enum.TryParse(equipmentNameString, out EquipmentName equipmentName);

                var quantity = request.Quantity;
                var timestamp = DateTime.Parse(request.TimeStamp);

                if ((DateTime.Now - timestamp).TotalHours >= 24)
                {
                    var lastId = _getHighestEquipmentId();
                    for (int i = 0; i < quantity; i++)
                    {
                        var requestedEquipment = new Equipment(
                            lastId + 1,
                            EquipmentType.Dynamic,
                            equipmentName,
                            100); // storage is 100

                        lastId++;

                        AddEquipment(requestedEquipment);
                    }
                }
                else
                {
                    newRequests.Add(request); // Keep the request for future processing
                }
            }

            // Overwrite the file with the requests that have not been processed yet
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
            var jsonString = JsonSerializer.Serialize(newRequests, options);
            File.WriteAllText(RequestFilePath, jsonString);
        }
        
        public bool TransferRequestExists(int equipmentId)
        {
            if (!File.Exists(TransferFilePath))
            {
                return false;
            }

            string jsonData = File.ReadAllText(TransferFilePath);
            var transfers = JsonSerializer.Deserialize<List<EquipmentTransfer>>(jsonData);

            foreach (var transfer in transfers)
            {
                if (transfer.EquipmentId == equipmentId)
                {
                    return true;
                }
            }

            return false;
        }
        
        // NOTE: This method overwrites old requests that have the same equipmentID
        public void WriteTransferToFile(int donorId, int targetId, int equipmentId, DateTime transferTime)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TransferFilePath));

            List<EquipmentTransfer> transfers = new List<EquipmentTransfer>();

            if (File.Exists(TransferFilePath))
            {
                string jsonData = File.ReadAllText(TransferFilePath);
                transfers = JsonSerializer.Deserialize<List<EquipmentTransfer>>(jsonData);
            }

            var newTransfer = new EquipmentTransfer(
                donorId,
                targetId,
                equipmentId,
                transferTime.ToString("yyyy-MM-dd HH:mm:ss")
            );

            bool found = false;

            // Iterate through the list of transfers and update the matching one
            for (int i = 0; i < transfers.Count; i++)
            {
                if (transfers[i].EquipmentId == equipmentId)
                {
                    transfers[i] = newTransfer;
                    found = true;
                    break;
                }
            }

            // If there's no matching transfer, add the new one
            if (!found)
            {
                transfers.Add(newTransfer);
            }

            string jsonString = JsonSerializer.Serialize(transfers);
            File.WriteAllText(TransferFilePath, jsonString);
        }
        
        private void ProcessEquipmentTransfers()
        {
            if (!File.Exists(TransferFilePath))
            {
                Console.WriteLine("No requests found.");
                return;
            }

            string jsonData = File.ReadAllText(TransferFilePath);
            var transfers = JsonSerializer.Deserialize<List<EquipmentTransfer>>(jsonData);

            List<EquipmentTransfer> newTransfers = new List<EquipmentTransfer>();

            var roomService = new RoomService();
            roomService.Refresh();
            _loadEquipmentFromJson();

            foreach (var transfer in transfers)
            {

                var transferTime = DateTime.Parse(transfer.TransferTime);

                if (DateTime.Now > transferTime)
                {
                    var donorRoom = roomService.GetRoomById(transfer.DonorId);
                    var targetRoom = roomService.GetRoomById(transfer.TargetId);

                    var eq = GetEquipmentById(transfer.EquipmentId);
                    
                    donorRoom.RemoveEquipment(eq);
                    targetRoom.AddEquipment(eq);
                    
                    roomService.UpdateRoom(targetRoom);
                    UpdateEquipmentLocationById(transfer.EquipmentId, targetRoom);
                }
                else
                {
                    newTransfers.Add(transfer); // Keep the request for future processing
                }
            }

            // Overwrite the file with the requests that have not been processed yet
            var jsonString = JsonSerializer.Serialize(newTransfers);
            File.WriteAllText(TransferFilePath, jsonString);
        }
    }
}

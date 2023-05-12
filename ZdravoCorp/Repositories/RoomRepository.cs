using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class RoomRepository {
    
        private List<Room> rooms;
        private string Path = @"..\\..\\..\\Data\\rooms.json";

        public void SetRooms(List<Room> data)
        {
            rooms = data;
        }
        
        public void LoadRoomsFromJson()
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
            string jsonData = File.ReadAllText(Path);
            rooms = JsonSerializer.Deserialize<List<Room>>(jsonData, options);
        }

        public void LoadRooms()
        {
            LoadRoomsFromJson();
        }

        private void _writeToJson()
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
            string jsonData = JsonSerializer.Serialize(rooms, options);
            File.WriteAllText(Path, jsonData);
        }

        public List<Room> GetAllRooms()
        {
            LoadRoomsFromJson();
            return rooms;
        }

        public bool RoomExists(int id)
        {
            return rooms.Any(x => x.Id == id);
        }

        public Room GetRoomById(int id)
        {
            if (RoomExists(id))
                return rooms.Find(x => x.Id == id);
            return null;
        }

        public Room GetFreeRoomOfType(RoomType type)
        {
            return rooms.Find(room => room.Type == type);
        }

        public List<Room> GetRoomByType(RoomType type)
        {
            return rooms.Where(x => x.Type == type).ToList();
        }

        public void UpdateRoom(Room newData)
        {
            if (RoomExists(newData.Id))
            {
                var index = rooms.FindIndex(x => x.Id == newData.Id);
                if (index != -1)
                {
                    rooms[index] = newData;
                    _writeToJson();
                }
            }
        }

        public void AddRoom(Room room)
        {
            if (!RoomExists(room.Id))
            {
                rooms.Add(room);
                _writeToJson();
            }
        }
    }
}

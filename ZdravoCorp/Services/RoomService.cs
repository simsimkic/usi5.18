using ZdravoCorp.Repositories;
using System.Collections.Generic;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Services;

public class RoomService
{
    private RoomRepository _roomRepository;

    public RoomService()
    {
        _roomRepository = new RoomRepository();
        _roomRepository.LoadRooms();
    }

    public List<Room> GetAllRooms()
    {
        return _roomRepository.GetAllRooms();
    }
    
    public Room GetRoomById(int id)
    {
        return _roomRepository.GetRoomById(id);
    }

    public void AddRoom(Room room)
    {
        _roomRepository.AddRoom(room);
    }

    public void UpdateRoom(Room newData)
    {
        _roomRepository.UpdateRoom(newData);
    }

    public List<Room> GetRoomByType(RoomType type)
    {
        return _roomRepository.GetRoomByType(type);
    }

    public bool RoomExists(int id)
    {
        return _roomRepository.RoomExists(id);
    }

    public void Refresh()
    {
        _roomRepository.LoadRoomsFromJson();
    }
}

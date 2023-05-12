using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;

namespace ZdravoCorp.Domain
{
    public class Hospital
    {
        public EquipmentService EquipmentService { get; set; }
        public RoomService RoomService { get; set; }
        
        public Hospital()
        {
            EquipmentService = new EquipmentService();
            EquipmentService.Refresh();
            
            RoomService = new RoomService();
            RoomService.Refresh();
        }
    }
}

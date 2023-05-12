using System.Collections.Generic;
namespace ZdravoCorp.Domain
{
    public enum EquipmentType
    {
        Examination,
        Operation,
        Furniture,
        Hallway,
        Dynamic
    }
    public enum EquipmentName
    {
        // Standard equipment
        SurgicalTable, Scalpel, SurgicalLight, AnesthesiaMachine, Electrocautery, Table,
        Chair, Bed, Cabinet, ExaminationTable, Stethoscope, BloodPressureMonitor, Otoscope,
        Thermometer, OverbedTable, IvStand, BedsideLamp, Sofa, MagazineRack, WaterDispenser,
        
        // Dynamic equipment
        Gauze, MedicalTape, Bandage, Syringe, Needle, Paper, Pen, Pencil, AlcoholWipes,
        DisposableGloves, FaceMask, CottonBalls, Tourniquet, TongueDepressor, AdhesiveBandage
    }
    
    public class Equipment
    {
        public int Id { get; set; }
        public EquipmentType Type { get; set; }
        public EquipmentName Name { get; set; }
        public int LocationId { get; set; }

        public Equipment(int id, EquipmentType type, EquipmentName name, int locationId)
        {
            Id = id;
            Type = type;
            Name = name;
            LocationId = locationId;
        }

        public static List<EquipmentName> GetStandardNames()
        {
            var names = new List<EquipmentName>();
            names.Add(EquipmentName.SurgicalTable);
            names.Add(EquipmentName.Scalpel);
            names.Add(EquipmentName.SurgicalLight);
            names.Add(EquipmentName.AnesthesiaMachine);
            names.Add(EquipmentName.Electrocautery);
            names.Add(EquipmentName.Table);
            names.Add(EquipmentName.Chair);
            names.Add(EquipmentName.Bed);
            names.Add(EquipmentName.Cabinet);
            names.Add(EquipmentName.ExaminationTable);
            names.Add(EquipmentName.Stethoscope);
            names.Add(EquipmentName.BloodPressureMonitor);
            names.Add(EquipmentName.Otoscope);
            names.Add(EquipmentName.Thermometer);
            names.Add(EquipmentName.OverbedTable);
            names.Add(EquipmentName.IvStand);
            names.Add(EquipmentName.BedsideLamp);
            names.Add(EquipmentName.Sofa);
            names.Add(EquipmentName.MagazineRack);
            names.Add(EquipmentName.WaterDispenser);

            return names;
        }
        
        public static List<EquipmentName> GetDynamicNames()
        {
            var names = new List<EquipmentName>();

            names.Add(EquipmentName.Gauze);
            names.Add(EquipmentName.MedicalTape);
            names.Add(EquipmentName.Bandage);
            names.Add(EquipmentName.Syringe);
            names.Add(EquipmentName.Needle);
            names.Add(EquipmentName.Paper);
            names.Add(EquipmentName.Pen);
            names.Add(EquipmentName.Pencil);
            names.Add(EquipmentName.AlcoholWipes);
            names.Add(EquipmentName.DisposableGloves);
            names.Add(EquipmentName.FaceMask);
            names.Add(EquipmentName.CottonBalls);
            names.Add(EquipmentName.Tourniquet);
            names.Add(EquipmentName.TongueDepressor);
            names.Add(EquipmentName.AdhesiveBandage);
            
            return names;
        }

        public override string ToString()
        {
            return "{Id: " + Id + ", " +
                   "Type: " + Type + ", " +
                   "Name: " + Name + ", " +
                   "Location: " + LocationId + "}";
        }
    }
}


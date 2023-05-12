namespace ZdravoCorp.Domain
{
    public class EquipmentTransfer
    {
        public int DonorId { get; set; }
        public int TargetId { get; set; }
        public int EquipmentId { get; set; }
        public string TransferTime { get; set; }

        public EquipmentTransfer(int donorId, int targetId, int equipmentId, string transferTime)
        {
            DonorId = donorId;
            TargetId = targetId;
            EquipmentId = equipmentId;
            TransferTime = transferTime;
        }
    }
}
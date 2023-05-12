namespace ZdravoCorp.Domain
{
    public class EquipmentRequest
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string TimeStamp { get; set; }

        public EquipmentRequest(string name, int quantity, string timestamp)
        {
            Name = name;
            Quantity = quantity;
            TimeStamp = timestamp;
        }
    }
}
namespace StudentBazaar.web.Models
{
    public class Shipment
    {
        public int ShipID { get; set; }
        public int OrderID { get; set; }
        public int ShipperID { get; set; }
        public string TrackingNumber { get; set; }
        public string ShipStatus { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.Now;
    }
}

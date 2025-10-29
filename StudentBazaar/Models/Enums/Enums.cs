namespace StudentBazaar.Web.Models.Enums
{
    public enum UserRole { Student, Shipper, Admin }
    public enum ListingCondition { New, Excellent, Good, Fair, Poor }
    public enum OrderStatus { Pending, Confirmed, Shipped, Delivered, Cancelled }
    public enum PaymentMethod { Online, CashOnDelivery }
    public enum ShipmentStatus { AwaitingPickup, InTransit, Delivered, Delayed, Failed }
    public enum ListingStatus {Available, Sold, Hidden,Reserved}
}

namespace StudentBazaar.Web.Models
{
    public enum UserRole { Student, Shipper, Admin }

    public enum ListingCondition
    {
        New,
        Excellent,
        Good,
        Fair,
        Poor
    }

    public enum orderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }

    public enum paymentMethod
    {
        Online,
        CashOnDelivery
    }

    public enum ShipmentStatus
    {
        AwaitingPickup,
        InTransit,
        Delivered,
        Delayed,
        Failed
    }

    public enum ListingStatus
    {
        Available,
        Sold,
        Hidden,
        Reserved
    }
}

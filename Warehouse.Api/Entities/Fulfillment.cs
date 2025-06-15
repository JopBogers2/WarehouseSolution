namespace Warehouse.Api.Entities
{
    public class Fulfillment
    {
        public Guid Id { get; set; }
        public string Platform { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        public string ServiceLevel { get; set; } = string.Empty;
        public string ShipmentId { get; set; } = string.Empty;
        public FulfillmentShippingAddress ShippingAddress { get; set; } = new();
        public FulfillmentPickupPointAddress? PickupPointAddress { get; set; }
        public ICollection<FulfillmentLine> Lines { get; set; } = new List<FulfillmentLine>();
        public ICollection<ReturnMerchandiseAuthorization> ReturnMerchandiseAuthorizations { get; set; } = new List<ReturnMerchandiseAuthorization>();
        public ICollection<Return> Returns { get; set; } = new List<Return>();
    }
}
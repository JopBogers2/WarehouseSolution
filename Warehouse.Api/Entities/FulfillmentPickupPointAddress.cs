namespace Warehouse.Api.Entities
{
    public class FulfillmentPickupPointAddress
    {
        public Guid Id { get; set; }
        public string PickupPointId { get; set; } = string.Empty;
        public string PickupPointName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
    }
}

namespace Warehouse.Shared.Models
{
    public class FulfillmentDto
    {
        public required string Platform { get; set; }
        public required string Channel { get; set; }
        public required string OrderId { get; set; }
        public required string Email { get; set; }
        public required FulfillmentShippingAddressDto ShippingAddress { get; set; }
        public FulfillmentPickupPointAddressDto? PickupPointAddress { get; set; }
        public required string Carrier { get; set; }
        public required string ServiceLevel { get; set; }
        public required string ShipmentId { get; set; }
        public required List<FulfillmentLineDto> Lines { get; set; }
    }

    public class FulfillmentShippingAddressDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
        public string? MobilePhone { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string ZipCode { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
    }

    public class FulfillmentPickupPointAddressDto
    {
        public required string PickupPointId { get; set; }
        public required string PickupPointName { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
    }

    public class FulfillmentLineDto
    {
        public required int LineId { get; set; }
        public required string DistributionCenter { get; set; }
        public required string ArticleCode { get; set; }
        public required int Quantity { get; set; }
        public required string FulfillmentType { get; set; }
        public required DateOnly ShippingDate { get; set; }
    }
}

namespace Warehouse.Api.Entities
{
    public class FulfillmentLine
    {
        public Guid Id { get; set; }
        public int LineId { get; set; }
        public string DistributionCenter { get; set; } = string.Empty;
        public string ArticleCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string FulfillmentType { get; set; } = string.Empty;
        public DateOnly ShippingDate { get; set; }
    }
}

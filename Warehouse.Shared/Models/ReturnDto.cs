namespace Warehouse.Shared.Models
{
    public class ReturnDto
    {
        public string? EventType { get; set; }
        public required string Platform { get; set; }
        public required string Channel { get; set; }
        public required string OrderId { get; set; }
        public required string ReturnRequestId { get; set; }
        public required List<ReturnLineDto> Lines { get; set; }
    }

    public class ReturnLineDto
    {
        public required int LineId { get; set; }
        public required string ArticleCode { get; set; }
        public required int Quantity { get; set; }
        public required string DistributionCenter { get; set; }
        public required string Reason { get; set; }
        public required string Condition { get; set; }
    }
}

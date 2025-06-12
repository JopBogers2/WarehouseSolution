namespace Warehouse.Shared.Models
{
    public class ReturnMerchandiseAuthorizationDto
    {
        public required string Platform { get; set; }
        public required string Channel { get; set; }
        public required string OrderId { get; set; }
        public required string ReturnRequestId { get; set; }
        public required string DistributionCenter { get; set; }
        public required string Currency { get; set; }
        public required List<string> TrackAndTrace { get; set; }
        public required List<ReturnMerchandiseAuthorizationLineDto> Lines { get; set; }
    }

    public class ReturnMerchandiseAuthorizationLineDto
    {
        public required int LineId { get; set; }
        public required string ArticleCode { get; set; }
        public required int Quantity { get; set; }
        public required string Reason { get; set; }
        public required string Resolution { get; set; }
    }
}

namespace Warehouse.Api.Entities
{
    public class ReturnLine
    {
        public Guid Id { get; set; }
        public int LineId { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string DistributionCenter { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

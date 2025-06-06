namespace Warehouse.Api.Entities
{
    public class ReturnMerchandiseAuthorizationLine
    {
        public Guid Id { get; set; }
        public int LineId { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
        public ReturnMerchandiseAuthorization ReturnMerchandiseAuthorization { get; set; } = null!;
    }
}

namespace Warehouse.Api.Entities
{
    public class Return
    {
        public Guid Id { get; set; }
        public string Platform { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string ReturnRequestId { get; set; } = string.Empty;
        public ICollection<ReturnLine> Lines { get; set; } = new List<ReturnLine>();
        public ICollection<ReturnMerchandiseAuthorization> ReturnMerchandiseAuthorizations { get; set; } = new List<ReturnMerchandiseAuthorization>();
        public ICollection<Fulfillment> Fulfillments { get; set; } = new List<Fulfillment>();

    }
}
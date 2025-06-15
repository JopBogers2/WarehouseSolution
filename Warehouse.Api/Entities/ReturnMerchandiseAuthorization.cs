namespace Warehouse.Api.Entities
{
    public class ReturnMerchandiseAuthorization
    {
        public Guid Id { get; set; }
        public string Platform { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string ReturnRequestId { get; set; } = string.Empty;
        public string DistributionCenter { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public ICollection<TrackAndTrace> TrackAndTraces { get; set; } = new List<TrackAndTrace>();
        public ICollection<ReturnMerchandiseAuthorizationLine> Lines { get; set; } = new List<ReturnMerchandiseAuthorizationLine>();
        public ICollection<Fulfillment> Fulfillments { get; set; } = new List<Fulfillment>();
        public ICollection<Return> Returns { get; set; } = new List<Return>();
    }
}

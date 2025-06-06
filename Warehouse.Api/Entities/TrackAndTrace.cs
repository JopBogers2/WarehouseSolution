namespace Warehouse.Api.Entities
{
    public class TrackAndTrace
    {
        public Guid Id { get; set; }
        public required string TrackAndTraceCode { get; set; } = string.Empty;
        public ReturnMerchandiseAuthorization ReturnMerchandiseAuthorization { get; set; } = null!;

    }
}

using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Entities;

namespace Warehouse.Api.Data
{
    public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) : DbContext(options)
    {
        public DbSet<Fulfillment> Fulfillments { get; set; } = null!;
        public DbSet<FulfillmentShippingAddress> FulfillmentShippingAddresses { get; set; } = null!;
        public DbSet<FulfillmentPickupPointAddress> FulfillmentPickupPointAddresses { get; set; }
        public DbSet<FulfillmentLine> FulfillmentLines { get; set; } = null!;
        public DbSet<ReturnMerchandiseAuthorization> ReturnMerchandiseAuthorizations { get; set; }
        public DbSet<ReturnMerchandiseAuthorizationLine> ReturnMerchandiseAuthorizationLines { get; set; }
        public DbSet<TrackAndTrace> TrackAndTraces { get; set; }
    }
}

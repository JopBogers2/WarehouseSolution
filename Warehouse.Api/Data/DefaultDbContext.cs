using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Entities;

namespace Warehouse.Api.Data
{
    public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) : DbContext(options)
    {
        public DbSet<ReturnMerchandiseAuthorization> ReturnMerchandiseAuthorization { get; set; }
        public DbSet<ReturnMerchandiseAuthorizationLine> ReturnMerchandiseAuthorizationLines { get; set; }
        public DbSet<TrackAndTrace> TrackAndTraces { get; set; }
    }
}

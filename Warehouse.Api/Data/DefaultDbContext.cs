using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Entities;

namespace Warehouse.Api.Data
{
    public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) : DbContext(options)
    {
        public DbSet<Return> Returns { get; set; }
    }
}

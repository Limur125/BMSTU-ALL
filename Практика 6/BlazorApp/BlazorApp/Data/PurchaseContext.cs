using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data
{
    public class PurchaseContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public DbSet<Material> Materials { get; set; }

        public PurchaseContext(DbContextOptions<PurchaseContext> options) : base(options) { }
    }
}

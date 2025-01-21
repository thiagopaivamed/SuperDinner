using Microsoft.EntityFrameworkCore;
using SuperDinner.Domain.Entities;

namespace SuperDinner.Infrastructure.Data.Context
{
    public sealed class SuperDinnerContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dinner> Dinners { get; set; }
        public SuperDinnerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SuperDinnerContext).Assembly);
        }
    }
}

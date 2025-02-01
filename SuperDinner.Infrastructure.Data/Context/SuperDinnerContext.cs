using Microsoft.EntityFrameworkCore;
using SuperDinner.Domain.Entities;

namespace SuperDinner.Infrastructure.Data.Context
{
    public sealed class SuperDinnerContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dinner> Dinners { get; set; }       

        public SuperDinnerContext(DbContextOptions options) : base(options)
            => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SuperDinnerContext).Assembly);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql("SuperDinnerConnection");

        }
    }
}

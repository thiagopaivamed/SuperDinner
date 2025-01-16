using Microsoft.EntityFrameworkCore;

namespace SuperDinner.Infrastructure.Data.Context
{
    public sealed class SuperDinnerContext : DbContext
    {
        public SuperDinnerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SuperDinnerContext).Assembly);
        }
    }
}

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace SuperDinner.Infrastructure.Data.Context
{
    public sealed class SuperDinnerContextFactory : IDesignTimeDbContextFactory<SuperDinnerContext>
    {
        public SuperDinnerContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SuperDinnerContext> optionsBuilder = new DbContextOptionsBuilder<SuperDinnerContext>();
            optionsBuilder.UseNpgsql("SuperDinnerConnection");

            return new SuperDinnerContext(optionsBuilder.Options);
        }
    }
}
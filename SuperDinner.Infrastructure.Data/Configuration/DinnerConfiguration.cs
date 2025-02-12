using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperDinner.Domain.Entities;

namespace SuperDinner.Infrastructure.Data.Configuration
{
    public sealed class DinnerConfiguration : IEntityTypeConfiguration<Dinner>
    {
        public void Configure(EntityTypeBuilder<Dinner> builder)
        {
            builder.ToTable("Dinners");

            builder.HasKey(d => d.DinnerId);
            builder.Property(d => d.DinnerDate).IsRequired();            
            builder.Property(d => d.DinnerStatus).IsRequired();
            builder.Property(d => d.CreatedDate).IsRequired();
            builder.Property(d => d.LastModifiedDate).IsRequired(false);

            builder.HasOne(d => d.Restaurant).WithMany(d => d.Dinners).HasForeignKey(d => d.RestaurantId).IsRequired();
        }
    }
}

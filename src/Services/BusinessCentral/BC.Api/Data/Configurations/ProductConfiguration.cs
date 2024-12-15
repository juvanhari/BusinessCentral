using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BC.Api.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Company).IsRequired();

            builder.Property(p => p.ItemNo).HasMaxLength(100).IsRequired();

        }
    }
}

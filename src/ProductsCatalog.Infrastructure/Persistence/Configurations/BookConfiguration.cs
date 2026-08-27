using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(b => b.Id);
        builder.Ignore(b => b.DomainEvents);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(30);
        builder.Property(b => b.Author).IsRequired().HasMaxLength(30);
        builder.Property(b => b.Genre).IsRequired().HasConversion<int>();
        builder.Property(b => b.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(b => b.CreatedAt).IsRequired();
        builder.Property(b => b.UpdatedAt).IsRequired();
    }
}

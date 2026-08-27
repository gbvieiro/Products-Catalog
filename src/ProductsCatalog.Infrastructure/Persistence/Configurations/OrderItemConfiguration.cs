using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.Property<Guid>("Id").IsRequired();
        builder.HasKey("Id");

        builder.Property<Guid>("OrderId").IsRequired();

        builder.Property(oi => oi.BookId).IsRequired();
        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.UnitPrice).IsRequired().HasPrecision(18, 2);

        // Amount = UnitPrice * Quantity e calculado em memoria (ver OrderItem.Amount), nao persistido.
        builder.Ignore(oi => oi.Amount);

        builder.HasOne<Book>()
            .WithMany()
            .HasForeignKey(oi => oi.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("OrderId");
    }
}

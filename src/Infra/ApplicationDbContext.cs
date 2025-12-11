using Microsoft.EntityFrameworkCore;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Enums;
using Products.Catalog.Domain.ValueObject;

namespace Infra.Databases;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Book
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");
            
            entity.HasKey(b => b.Id);
            
            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(b => b.Genre)
                .IsRequired()
                .HasConversion<int>();
            
            entity.Property(b => b.Price)
                .IsRequired()
                .HasPrecision(18, 2);
            
            entity.Property(b => b.CreatedAt)
                .IsRequired();
            
            entity.Property(b => b.UpdateAt)
                .IsRequired();
        });

        // Configuração da entidade User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            
            entity.HasKey(u => u.Id);
            
            // Configurar Email como owned entity
            entity.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(255);
                
                // Índice único para Email (dentro do owned entity)
                email.HasIndex(e => e.Address)
                    .IsUnique();
            });
            
            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(u => u.CreatedAt)
                .IsRequired();
            
            entity.Property(u => u.UpdateAt)
                .IsRequired();
        });

        // Configuração da entidade Stock
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("Stocks");
            
            entity.HasKey(s => s.Id);
            
            entity.Property(s => s.Quantity)
                .IsRequired();
            
            entity.Property(s => s.BookId)
                .IsRequired();
            
            entity.Property(s => s.CreatedAt)
                .IsRequired();
            
            entity.Property(s => s.UpdateAt)
                .IsRequired();
            
            // Relacionamento com Book
            entity.HasOne<Book>()
                .WithMany()
                .HasForeignKey(s => s.BookId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Índice único para BookId (um estoque por livro)
            entity.HasIndex(s => s.BookId)
                .IsUnique();
        });

        // Configuração da entidade Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            
            entity.HasKey(o => o.Id);
            
            entity.Property(o => o.CustomerId)
                .IsRequired();
            
            entity.Property(o => o.Status)
                .IsRequired()
                .HasConversion<int>();
            
            entity.Property(o => o.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);
            
            entity.Property(o => o.CreatedAt)
                .IsRequired();
            
            entity.Property(o => o.UpdateAt)
                .IsRequired();
            
            // Relacionamento com User (Customer)
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relacionamento com OrderItems
            entity.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);
            
            // Índice para CustomerId para melhor performance em consultas
            entity.HasIndex(o => o.CustomerId);
            
            // Índice para Status
            entity.HasIndex(o => o.Status);
        });

        // Configuração da entidade OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");
            
            // Shadow property para Id (já que OrderItem não herda de Entity)
            entity.Property<Guid>("Id")
                .IsRequired();
            
            entity.HasKey("Id");
            
            // Shadow property para OrderId (relacionamento com Order)
            entity.Property<Guid>("OrderId")
                .IsRequired();
            
            entity.Property(oi => oi.BookId)
                .IsRequired();
            
            entity.Property(oi => oi.Quantity)
                .IsRequired();
            
            entity.Property(oi => oi.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            
            // Relacionamento com Book
            entity.HasOne<Book>()
                .WithMany()
                .HasForeignKey(oi => oi.BookId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Índice para OrderId (shadow property) para melhor performance
            entity.HasIndex("OrderId");
        });
    }
}


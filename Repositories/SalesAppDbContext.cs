using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories;

public class SalesAppDbContext : DbContext
{
    public SalesAppDbContext(DbContextOptions<SalesAppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<StoreLocation> StoreLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình cho Entity User
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.Username).IsUnique(); 

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.PhoneNumber).HasMaxLength(15);
            builder.Property(u => u.Address).HasMaxLength(255);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasKey(c => c.CategoryId);
            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId);
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.BriefDescription).HasMaxLength(255);
            builder.Property(p => p.ImageUrl).HasMaxLength(255);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Cart>(builder =>
        {
            builder.HasKey(c => c.CartId);
            builder.Property(c => c.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(c => c.CartItems)
                   .WithOne(ci => ci.Cart)
                   .HasForeignKey(ci => ci.CartId);
        });

        modelBuilder.Entity<CartItem>(builder =>
        {
            builder.HasKey(ci => ci.CartItemId);
            builder.Property(ci => ci.Quantity).IsRequired();
            builder.Property(ci => ci.Price)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Order>(builder =>
        {
            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.BillingAddress)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(o => o.OrderStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.OrderDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasMany(o => o.Payments)
                   .WithOne(p => p.Order)
                   .HasForeignKey(p => p.OrderId);
        });

        modelBuilder.Entity<Payment>(builder =>
        {
            builder.HasKey(p => p.PaymentId);
            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.PaymentDate)
                .HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Notification>(builder =>
        {
            builder.HasKey(n => n.NotificationId);
            builder.Property(n => n.Message).HasMaxLength(255);
            builder.Property(n => n.IsRead).HasDefaultValue(false);
            builder.Property(n => n.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<ChatMessage>(builder =>
        {
            builder.HasKey(cm => cm.ChatMessageId);
            builder.Property(cm => cm.SentAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<StoreLocation>(builder =>
        {
            builder.HasKey(sl => sl.LocationId);
            builder.Property(sl => sl.Latitude)
                .IsRequired()
                .HasColumnType("decimal(9, 6)");

            builder.Property(sl => sl.Longitude)
                .IsRequired()
                .HasColumnType("decimal(9, 6)");

            builder.Property(sl => sl.Address)
                .IsRequired()
                .HasMaxLength(255);
        });
    }
}


using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HoshiVibe.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Zodiac> Zodiacs { get; set; }
        public DbSet<Destiny> Destinies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentTransactions> PaymentTransactions { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CustomProduct> CustomProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================== PRIMARY KEYS ==================
            modelBuilder.Entity<User>().HasKey(u => u.User_Id);
            modelBuilder.Entity<UserProfile>().HasKey(up => up.UserProfile_Id);
            modelBuilder.Entity<Zodiac>().HasKey(z => z.Id);
            modelBuilder.Entity<Destiny>().HasKey(d => d.Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Product_Id);
            modelBuilder.Entity<Voucher>().HasKey(v => v.Voucher_Id);
            modelBuilder.Entity<Order>().HasKey(o => o.Order_Id);
            modelBuilder.Entity<OrderDetail>().HasKey(od => od.OrderDetail_Id);
            modelBuilder.Entity<Payment>().HasKey(p => p.Payment_Id);
            modelBuilder.Entity<PaymentTransactions>().HasKey(pt => pt.Transaction_Id);
            modelBuilder.Entity<Cart>().HasKey(c => c.Cart_Id);
            modelBuilder.Entity<CartItem>().HasKey(ci => ci.CartItem_Id);
            modelBuilder.Entity<CustomProduct>().HasKey(cp => cp.CProduct_Id);

            // ================== RELATIONSHIPS ==================

            // USER - USERPROFILE (1-1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N Destiny - UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.Destiny)
                .WithMany(d => d.UserProfiles)
                .HasForeignKey(up => up.DestinyId)
                .OnDelete(DeleteBehavior.SetNull);

            // 1:N Zodiac - UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.Zodiac)
                .WithMany(z => z.UserProfiles)
                .HasForeignKey(up => up.ZodiacId)
                .OnDelete(DeleteBehavior.SetNull);

            // USER - ORDER (1-n)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.User_Id)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomProduct>(e =>
            {
                e.ToTable("CustomProduct");           
                e.Property(p => p.Price).HasPrecision(18, 2);
                e.HasOne(p => p.User)
                   .WithMany()
                  .HasForeignKey(p => p.User_Id)
                  .HasPrincipalKey(u => u.User_Id)
                  .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CustomProduct>(entity =>
            {
                entity.Property(p => p.Price).HasPrecision(18, 2);
            });


            // ORDER - ORDER DETAIL (1-n)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ORDER - PAYMENT (1-1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.Order_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // PAYMENT - TRANSACTION (1-n)
            modelBuilder.Entity<Payment>()
                .HasMany(p => p.Transactions)
                .WithOne(pt => pt.Payment)
                .HasForeignKey(pt => pt.Payment_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // ORDER - VOUCHER (n-1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Voucher)
                .WithMany(v => v.Orders)
                .HasForeignKey(o => o.Voucher_Id)
                .OnDelete(DeleteBehavior.SetNull);

            // ORDER DETAIL - PRODUCT (n-1)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // SHOPPINGCART - USER (1-1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // SHOPPINGCART - ITEM (1-n)
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.Cart_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // CART ITEM - PRODUCT (n-1)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.Product_Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>(e =>
            {
                e.HasOne(o => o.Cart)
                 .WithMany()
                 .HasForeignKey(o => o.Cart_Id)
                 .HasPrincipalKey(c => c.Cart_Id);
            });

            // Handle decimal precision for monetary values
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalPrice).HasPrecision(18, 2);
                entity.Property(o => o.DiscountAmount).HasPrecision(18, 2);
                entity.Property(o => o.FinalPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(od => od.UnitPrice).HasPrecision(18, 2);
                entity.Property(od => od.Discount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Amount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(ci => ci.UnitPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(v => v.DiscountAmount).HasPrecision(18, 2);
            });
        }
    }
}

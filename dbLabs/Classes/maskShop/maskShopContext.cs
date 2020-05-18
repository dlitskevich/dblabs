using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace dbLabsDummy.maskShop
{
    public partial class maskShopContext : DbContext
    {
        public maskShopContext()
        {
        }

        public maskShopContext(DbContextOptions<maskShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contracts> Contracts { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<Purchases> Purchases { get; set; }
        public virtual DbSet<ShopItems> ShopItems { get; set; }
        public virtual DbSet<Staffs> Staffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=127.0.0.1;database=maskShop;user=root;password=Password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contracts>(entity =>
            {
                entity.ToTable("Contracts", "maskShop");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.End).HasColumnType("date");

                entity.Property(e => e.Start).HasColumnType("date");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.ToTable("Customers", "maskShop");

                entity.HasIndex(e => e.Id)
                    .HasName("Id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CustomerType)
                    .IsRequired()
                    .HasColumnName("Customer_type");

                entity.Property(e => e.Discount).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Products", "maskShop");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'goodproduct'");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("provider", "maskShop");

                entity.Property(e => e.Id).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Purchases>(entity =>
            {
                entity.ToTable("Purchases", "maskShop");

                entity.HasIndex(e => e.CustomerId);

                entity.HasIndex(e => e.ShopItemId);

                entity.HasIndex(e => e.StaffId);

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Amount).HasColumnType("int(11)");

                entity.Property(e => e.CustomerId).HasColumnType("int(11)");

                entity.Property(e => e.ShopItemId).HasColumnType("int(11)");

                entity.Property(e => e.StaffId).HasColumnType("int(11)");

                entity.Property(e => e.Time).HasColumnType("date");
            });

            modelBuilder.Entity<ShopItems>(entity =>
            {
                entity.ToTable("ShopItems", "maskShop");

                entity.HasIndex(e => e.ProviderId);

                entity.HasIndex(e => new { e.ProductId, e.ProviderId });

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Amount).HasColumnType("int(11)");

                entity.Property(e => e.Happiness)
                    .IsRequired()
                    .HasColumnType("enum('Happy','Sad')")
                    .HasDefaultValueSql("'Happy'");

                entity.Property(e => e.Price).HasColumnType("int(11)");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.ProviderId).HasColumnType("int(11)");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ShopItems)
                    .HasForeignKey(d => d.ProviderId);
            });

            modelBuilder.Entity<Staffs>(entity =>
            {
                entity.ToTable("Staffs", "maskShop");

                entity.Property(e => e.Id).HasColumnType("int(11)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

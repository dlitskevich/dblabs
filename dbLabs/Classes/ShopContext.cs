using System;
using System.Collections;

using dbLabs;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;

namespace dbLabs.Classes {
    public class ShopContext : DbContext {

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<VIP> VIPs { get; set; }

        public DbSet<Provider> Providers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
		public DbSet<Purchase> Purchases { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySQL(
                "server=127.0.0.1;database=maskShop;user=root;password=Password");
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Provider>().ToTable("provider");

            modelBuilder.Entity<Contract>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Staff).WithOne(c => c.Contract).HasForeignKey<Staff>(s => s.Id).OnDelete(DeleteBehavior.Cascade); 
            });


            modelBuilder.Entity<ShopItem>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductId, e.ProviderId });
            });

            modelBuilder.Entity<Staff>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Purchase).WithOne(p => p.Staff).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Customer>()
				.HasDiscriminator<string>("Customer_type")
				.HasValue<Customer>("customer")
				.HasValue<VIP>("VIP");

            modelBuilder.Entity<VIP>();

            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50).HasDefaultValue("goodproduct");
            modelBuilder.Entity<ShopItem>().Property(c => c.Happiness).HasDefaultValue((Happiness)1).HasConversion<string>();
            modelBuilder.Entity<Purchase>().Property(p => p.Date).HasColumnType("date").HasColumnName("Time");

        }
        
        public bool MakePurchase(ShopItem item, int amount) {
            if(item.Buy(amount)) {
                Purchases.Add(new Purchase { ShopItem = item, Amount = amount, CustomerId = 1, StaffId = 1 });
                SaveChanges();
                return true;
            }
            return false;

        }

    }
}
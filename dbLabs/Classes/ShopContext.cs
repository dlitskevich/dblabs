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

        public DbSet<Provider> Providers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
		public DbSet<Purchase> Purchases { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySQL("server=127.0.0.1;database=maskShop;user=root;password=Password");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contract>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Staff).WithOne(c => c.Contract).HasForeignKey<Staff>(s => s.Id); 
            });

            modelBuilder.Entity<ShopItem>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductId, e.ProviderId });
            });

            /*
            modelBuilder.Entity<Candidate>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.Rating);
                entity.HasOne(e => e.CandidateProfile).WithOne(c => c.Candidate).HasForeignKey<CandidateProfile>(b => b.CandidateRef);
                //entity.HasMany(e => e.Confidents).WithOne(e => e.Candidate);
                //entity.HasMany(e => e.Promises).;
            });
			

            modelBuilder.Entity<CandidateProfile>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Age).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.HasOne(d => d.Candidate);
            });
			*/
            /*
            modelBuilder.Entity<Promise>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired();
                entity.HasMany(e => e.Candidates);
            });

            modelBuilder.Entity<Confident>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.PoliticalPreferences).IsRequired();
                entity.Property(e => e.Age).IsRequired();
                entity.HasOne(e => e.Candidate);
            });

            */
        }
    
    }
}
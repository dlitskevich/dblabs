using System;

using System.Collections;

using dbLabs.Classes;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;

namespace dbLabs {
    class ElectionContext : DbContext {
        //ConfigurationManager.ConnectionStrings["mmfConString"].ConnectionString
        //"server=127.0.0.1; user id=root; password=Password; database=weapon"
        //"data source=(localdb)\\MSSQLLocalDB;Initial Catalog=Election;Integrated Security=True;"
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateProfile> CandidateProfiles { get; set; }
        public DbSet<Promise> Promises { get; set; }
        public DbSet<Confident> Confidents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySQL("server=127.0.0.1;database=maskShop;user=root;password=Password");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CandidatePromise>(entity => {
                entity.HasKey(e => new { e.CandidateId, e.PromiseId });                
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
			*/
			
            modelBuilder.Entity<CandidateProfile>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Age).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.HasOne(d => d.Candidate);
            });
			
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
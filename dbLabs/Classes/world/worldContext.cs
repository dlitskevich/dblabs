using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace dbLabs.world
{
    public partial class worldContext : DbContext
    {
        public worldContext()
        {
        }

        public worldContext(DbContextOptions<worldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Countrylanguage> Countrylanguage { get; set; }
        public virtual DbSet<EconomicalMerkel> EconomicalMerkel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=127.0.0.1;database=world;user=root;password=Password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.HasIndex(e => e.CountryCode)
                    .HasName("CountryCode");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Population).HasColumnType("int(11)");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("city_ibfk_1");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");

                entity.ToTable("country");

                entity.Property(e => e.Code)
                    .HasMaxLength(3)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Capital).HasColumnType("int(11)");

                entity.Property(e => e.Code2)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Continent)
                    .IsRequired()
                    .HasColumnType("enum('Asia','Europe','North America','Africa','Oceania','Antarctica','South America')")
                    .HasDefaultValueSql("'Asia'");

                entity.Property(e => e.Gnp)
                    .HasColumnName("GNP")
                    .HasColumnType("float(10,2)");

                entity.Property(e => e.Gnpold)
                    .HasColumnName("GNPOld")
                    .HasColumnType("float(10,2)");

                entity.Property(e => e.GovernmentForm)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.HeadOfState)
                    .HasMaxLength(60)
                    .IsFixedLength();

                entity.Property(e => e.IndepYear).HasColumnType("smallint(6)");

                entity.Property(e => e.LifeExpectancy).HasColumnType("float(3,1)");

                entity.Property(e => e.LocalName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(52)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Population).HasColumnType("int(11)");

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasMaxLength(26)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.SurfaceArea).HasColumnType("float(10,2)");
            });

            modelBuilder.Entity<Countrylanguage>(entity =>
            {
                entity.HasKey(e => new { e.CountryCode, e.Language })
                    .HasName("PRIMARY");

                entity.ToTable("countrylanguage");

                entity.HasIndex(e => e.CountryCode)
                    .HasName("CountryCode");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(3)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Language)
                    .HasMaxLength(30)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.IsOfficial)
                    .IsRequired()
                    .HasColumnType("enum('T','F')")
                    .HasDefaultValueSql("'F'");

                entity.Property(e => e.Percentage).HasColumnType("float(4,1)");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Countrylanguage)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("countryLanguage_ibfk_1");
            });

            modelBuilder.Entity<EconomicalMerkel>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("economical_merkel");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(52)
                    .IsFixedLength()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Vnp)
                    .HasColumnName("VNP")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VnpNaDushuNaselenia)
                    .HasColumnName("VNP_na_dushu_naselenia")
                    .HasColumnType("double(22,6)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

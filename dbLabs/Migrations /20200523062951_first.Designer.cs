﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dbLabs.Classes;

namespace dbLabsDummy.Migrations
{
    [DbContext(typeof(ShopContext))]
    [Migration("20200523062951_first")]
    partial class first
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("dbLabs.Classes.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("End")
                        .HasColumnType("datetime");

                    b.Property<string>("Info")
                        .HasColumnType("text");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("dbLabs.Classes.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Customer_Type")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("dbLabs.Classes.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .HasDefaultValue("goodproduct");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("dbLabs.Classes.Provider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Info")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("provider");
                });

            modelBuilder.Entity("dbLabs.Classes.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Time")
                        .HasColumnType("date");

                    b.Property<int?>("ShopItemId")
                        .HasColumnType("int");

                    b.Property<int?>("StaffId")
                        .HasColumnType("int");

                    b.Property<int?>("TestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ShopItemId");

                    b.HasIndex("StaffId");

                    b.HasIndex("TestId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("dbLabs.Classes.ShopItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Happiness")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("enum('Happy','Sad')")
                        .HasDefaultValue("Happy");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ProviderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.HasIndex("ProductId", "ProviderId");

                    b.ToTable("ShopItems");
                });

            modelBuilder.Entity("dbLabs.Classes.Staff", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("dbLabs.Classes.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("Test_Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("dbLabs.Classes.Purchase", b =>
                {
                    b.HasOne("dbLabs.Classes.Customer", "Customer")
                        .WithMany("Purchase")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dbLabs.Classes.ShopItem", "ShopItem")
                        .WithMany("Purchase")
                        .HasForeignKey("ShopItemId");

                    b.HasOne("dbLabs.Classes.Staff", "Staff")
                        .WithMany("Purchase")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("dbLabs.Classes.Test", null)
                        .WithMany("Purchase")
                        .HasForeignKey("TestId");
                });

            modelBuilder.Entity("dbLabs.Classes.ShopItem", b =>
                {
                    b.HasOne("dbLabs.Classes.Product", "Product")
                        .WithMany("ShopItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dbLabs.Classes.Provider", "Provider")
                        .WithMany("ShopItems")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dbLabs.Classes.Staff", b =>
                {
                    b.HasOne("dbLabs.Classes.Contract", "Contract")
                        .WithOne("Staff")
                        .HasForeignKey("dbLabs.Classes.Staff", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using App12.SQLite.HotelDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App12.SQLite.Migrations
{
    [DbContext(typeof(HotelContext))]
    partial class HotelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("App12.SQLite.Models.Coupon", b =>
                {
                    b.Property<string>("CouponId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ClientPersonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CouponName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Discount")
                        .HasColumnType("INTEGER");

                    b.HasKey("CouponId");

                    b.HasIndex("ClientPersonId");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("App12.SQLite.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("PersonId");

                    b.ToTable("Persons");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Person");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("App12.SQLite.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("App12.SQLite.Models.Client", b =>
                {
                    b.HasBaseType("App12.SQLite.Models.Person");

                    b.Property<string>("Account")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RoomId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("RoomId");

                    b.HasDiscriminator().HasValue("Client");
                });

            modelBuilder.Entity("App12.SQLite.Models.Coupon", b =>
                {
                    b.HasOne("App12.SQLite.Models.Client", "Client")
                        .WithMany("Coupons")
                        .HasForeignKey("ClientPersonId");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("App12.SQLite.Models.Client", b =>
                {
                    b.HasOne("App12.SQLite.Models.Room", "Room")
                        .WithMany("Clients")
                        .HasForeignKey("RoomId");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("App12.SQLite.Models.Room", b =>
                {
                    b.Navigation("Clients");
                });

            modelBuilder.Entity("App12.SQLite.Models.Client", b =>
                {
                    b.Navigation("Coupons");
                });
#pragma warning restore 612, 618
        }
    }
}

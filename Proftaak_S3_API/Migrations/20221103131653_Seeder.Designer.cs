﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Proftaak_S3_API.Models;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    [DbContext(typeof(ProftaakContext))]
    [Migration("20221103131653_Seeder")]
    partial class Seeder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Proftaak_S3_API.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Kenteken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.Garage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("ClosingTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("MaxPrice")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<int>("MaxSpace")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("NormalPrice")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<DateTime?>("OpeningTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Garage");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MaxPrice = 5m,
                            MaxSpace = 5,
                            Name = "Test",
                            NormalPrice = 0m
                        });
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.Pricing", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("EndingTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GarageID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<DateTime>("StartingTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Pricing");
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.Receipt", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<decimal>("Price")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<int>("ReservationID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Receipt");
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.Reservations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CarID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SpaceID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "User"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.Space", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<int>("GarageID")
                        .HasColumnType("int");

                    b.Property<string>("Row")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Spot")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Space");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Floor = 1,
                            GarageID = 1,
                            Row = "a",
                            Spot = 1,
                            TypeId = 1
                        },
                        new
                        {
                            ID = 2,
                            Floor = 1,
                            GarageID = 1,
                            Row = "a",
                            Spot = 2,
                            TypeId = 1
                        },
                        new
                        {
                            ID = 3,
                            Floor = 1,
                            GarageID = 1,
                            Row = "a",
                            Spot = 3,
                            TypeId = 1
                        },
                        new
                        {
                            ID = 4,
                            Floor = 1,
                            GarageID = 1,
                            Row = "b",
                            Spot = 1,
                            TypeId = 1
                        },
                        new
                        {
                            ID = 5,
                            Floor = 2,
                            GarageID = 1,
                            Row = "a",
                            Spot = 1,
                            TypeId = 1
                        });
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.SpaceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SpaceType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Normal"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Electric"
                        });
                });

            modelBuilder.Entity("Proftaak_S3_API.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}

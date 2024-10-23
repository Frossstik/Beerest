﻿// <auto-generated />
using System;
using Beerest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Beerest.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241022015722_One_to_many")]
    partial class One_to_many
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Beerest.Models.Bars", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("bars");
                });

            modelBuilder.Entity("Beerest.Models.Beers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BeerIds")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Volume")
                        .HasColumnType("int");

                    b.Property<int>("VolumeAlc")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BeerIds");

                    b.ToTable("beers");
                });

            modelBuilder.Entity("Beerest.Models.Persons", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("BarId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BarId");

                    b.ToTable("persons");
                });

            modelBuilder.Entity("Beerest.Models.Beers", b =>
                {
                    b.HasOne("Beerest.Models.Bars", null)
                        .WithMany("Beers")
                        .HasForeignKey("BeerIds");
                });

            modelBuilder.Entity("Beerest.Models.Persons", b =>
                {
                    b.HasOne("Beerest.Models.Bars", "Bar")
                        .WithMany()
                        .HasForeignKey("BarId");

                    b.Navigation("Bar");
                });

            modelBuilder.Entity("Beerest.Models.Bars", b =>
                {
                    b.Navigation("Beers");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelephoneShop.Data;

#nullable disable

namespace TelephoneShop.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240718084306_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TelephoneShop.Models.Catalog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentCatalogId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentCatalogId");

                    b.ToTable("Catalog");
                });

            modelBuilder.Entity("TelephoneShop.Models.Cities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("TelephoneShop.Models.CitiesToTelephoneCost", b =>
                {
                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<int>("TelephoneId")
                        .HasColumnType("integer");

                    b.HasIndex("CityId");

                    b.HasIndex("TelephoneId");

                    b.ToTable("CitiesToTelephoneCost");
                });

            modelBuilder.Entity("TelephoneShop.Models.Telephone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CatalogId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.ToTable("Telephone");
                });

            modelBuilder.Entity("TelephoneShop.Models.Catalog", b =>
                {
                    b.HasOne("TelephoneShop.Models.Catalog", "ParentCatalog")
                        .WithMany()
                        .HasForeignKey("ParentCatalogId");

                    b.Navigation("ParentCatalog");
                });

            modelBuilder.Entity("TelephoneShop.Models.CitiesToTelephoneCost", b =>
                {
                    b.HasOne("TelephoneShop.Models.Cities", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TelephoneShop.Models.Telephone", "Telephone")
                        .WithMany()
                        .HasForeignKey("TelephoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Telephone");
                });

            modelBuilder.Entity("TelephoneShop.Models.Telephone", b =>
                {
                    b.HasOne("TelephoneShop.Models.Catalog", "Catalog")
                        .WithMany()
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Catalog");
                });
#pragma warning restore 612, 618
        }
    }
}

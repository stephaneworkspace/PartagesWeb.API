﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PartagesWeb.API.Data;

namespace PartagesWeb.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PartagesWeb.API.Models.Icone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FaValeur");

                    b.Property<string>("NomSelectBox");

                    b.HasKey("Id");

                    b.ToTable("Icones");
                });

            modelBuilder.Entity("PartagesWeb.API.Models.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Icone");

                    b.Property<string>("Nom");

                    b.Property<int>("Position");

                    b.Property<bool>("SwHorsLigne");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("PartagesWeb.API.Models.SousTitreMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nom");

                    b.Property<int>("Position");

                    b.Property<int?>("SectionId");

                    b.HasKey("Id");

                    b.ToTable("SousTitreMenus");
                });

            modelBuilder.Entity("PartagesWeb.API.Models.TitreMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nom");

                    b.Property<int>("Position");

                    b.Property<int?>("SectionId");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("TitreMenus");
                });

            modelBuilder.Entity("PartagesWeb.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PartagesWeb.API.Models.TitreMenu", b =>
                {
                    b.HasOne("PartagesWeb.API.Models.Section")
                        .WithMany("TitreMenus")
                        .HasForeignKey("SectionId");
                });
#pragma warning restore 612, 618
        }
    }
}

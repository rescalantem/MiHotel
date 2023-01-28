﻿// <auto-generated />
using System;
using MiHotel.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MiHotel.WebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230127195022_Inicial")]
    partial class Inicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MiHotel.Common.Entities.Acceso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EstanciaHabitacionId")
                        .HasColumnType("int");

                    b.Property<int>("EstanciaHuespedId")
                        .HasColumnType("int");

                    b.Property<Guid>("EstanciaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EstanciaHabitacionId", "EstanciaHuespedId");

                    b.ToTable("Accesos");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Estancia", b =>
                {
                    b.Property<int>("HabitacionId")
                        .HasColumnType("int");

                    b.Property<int>("HuespedId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Alta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Baja")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HabitacionId", "HuespedId");

                    b.HasIndex("HuespedId");

                    b.ToTable("Estancias");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Habitacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Foto")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<string>("NumeroStr")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("Ocupada")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Habitaciones");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RazonSocial")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Hoteles");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Huesped", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Telefono")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("Telefono");

                    b.ToTable("Huespedes");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Acceso", b =>
                {
                    b.HasOne("MiHotel.Common.Entities.Estancia", "Estancia")
                        .WithMany()
                        .HasForeignKey("EstanciaHabitacionId", "EstanciaHuespedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estancia");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Estancia", b =>
                {
                    b.HasOne("MiHotel.Common.Entities.Habitacion", "Habitacion")
                        .WithMany("Estancias")
                        .HasForeignKey("HabitacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiHotel.Common.Entities.Huesped", "Huesped")
                        .WithMany("Estancias")
                        .HasForeignKey("HuespedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Habitacion");

                    b.Navigation("Huesped");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Habitacion", b =>
                {
                    b.HasOne("MiHotel.Common.Entities.Hotel", "Hotel")
                        .WithMany("Habitaciones")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Habitacion", b =>
                {
                    b.Navigation("Estancias");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Hotel", b =>
                {
                    b.Navigation("Habitaciones");
                });

            modelBuilder.Entity("MiHotel.Common.Entities.Huesped", b =>
                {
                    b.Navigation("Estancias");
                });
#pragma warning restore 612, 618
        }
    }
}

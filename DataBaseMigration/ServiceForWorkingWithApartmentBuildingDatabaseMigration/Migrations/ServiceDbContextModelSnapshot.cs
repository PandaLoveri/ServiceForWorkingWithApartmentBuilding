﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceForWorkingWithApartmentBuildingDatabaseMigration;

namespace ServiceForWorkingWithApartmentBuildingDatabaseMigration.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    partial class ServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.Announcement", b =>
                {
                    b.Property<Guid>("AnnouncementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("character varying(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("Title")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.HasKey("AnnouncementId");

                    b.ToTable("Announcement");
                });

            modelBuilder.Entity("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.AnnouncementTenant", b =>
                {
                    b.Property<Guid>("TenatId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnnouncementId")
                        .HasColumnType("uuid");

                    b.HasKey("TenatId", "AnnouncementId");

                    b.HasIndex("AnnouncementId");

                    b.ToTable("AnnouncementTenant");
                });

            modelBuilder.Entity("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.Building", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<Guid>("ManagementCompanyId")
                        .HasColumnType("uuid");

                    b.HasKey("BuildingId");

                    b.HasIndex("ManagementCompanyId");

                    b.ToTable("Building");
                });

            modelBuilder.Entity("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.ManagementCompany", b =>
                {
                    b.Property<Guid>("ManagementCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("bytea");

                    b.Property<string>("Info")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.HasKey("ManagementCompanyId");

                    b.ToTable("ManagementCompany");
                });

            modelBuilder.Entity("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.Tenant", b =>
                {
                    b.Property<Guid>("TenatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("bytea");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EntranceNumber")
                        .HasColumnType("integer");

                    b.Property<int>("FlatNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Password")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Surname")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.HasKey("TenatId");

                    b.ToTable("Tenant");
                });

            modelBuilder.Entity("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.AnnouncementTenant", b =>
                {
                    b.HasOne("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.Announcement", null)
                        .WithMany("AnnouncementTenant")
                        .HasForeignKey("AnnouncementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.Tenant", null)
                        .WithMany("AnnouncementTenant")
                        .HasForeignKey("TenatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.Building", b =>
                {
                    b.HasOne("ServiceForWorkingWithApartmentBuildingDatabaseMigration.Entytes.ManagementCompany", null)
                        .WithMany("Buildings")
                        .HasForeignKey("ManagementCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

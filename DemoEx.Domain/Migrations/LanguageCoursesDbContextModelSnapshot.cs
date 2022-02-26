﻿// <auto-generated />
using System;
using DemoEx.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoEx.Domain.Migrations
{
    [DbContext(typeof(LanguageCoursesDbContext))]
    partial class LanguageCoursesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("DemoEx.Domain.Models.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("GenderName")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("Gender");

                    b.HasKey("Id");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.LanguageService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal?>("Cost")
                        .HasColumnType("money");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .HasMaxLength(200)
                        .HasColumnType("nchar(200)")
                        .IsFixedLength(true);

                    b.Property<string>("ServiceName")
                        .HasMaxLength(200)
                        .HasColumnType("nchar(200)")
                        .IsFixedLength(true);

                    b.HasKey("Id");

                    b.ToTable("LaguageServices");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.LanguageServiceImages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<int?>("LanguageServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("LanguageServiceId");

                    b.ToTable("LanguageServiceImages");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("GenderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nchar(50)")
                        .IsFixedLength(true);

                    b.Property<DateTime?>("RegistarionDate")
                        .HasColumnType("date");

                    b.Property<string>("Surname")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.ServiceRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServiceRecords");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.LanguageServiceImages", b =>
                {
                    b.HasOne("DemoEx.Domain.Models.Image", "Image")
                        .WithMany("LanguageServiceImages")
                        .HasForeignKey("ImageId");

                    b.HasOne("DemoEx.Domain.Models.LanguageService", "LanguageService")
                        .WithMany("LanguageServiceImages")
                        .HasForeignKey("LanguageServiceId");

                    b.Navigation("Image");

                    b.Navigation("LanguageService");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.Person", b =>
                {
                    b.HasOne("DemoEx.Domain.Models.Gender", "Gender")
                        .WithMany("People")
                        .HasForeignKey("GenderId")
                        .HasConstraintName("FK_Persons_Genders")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Gender");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.ServiceRecord", b =>
                {
                    b.HasOne("DemoEx.Domain.Models.Person", "Person")
                        .WithMany("ServiceRecords")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK_ServiceRecords_Persons")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DemoEx.Domain.Models.LanguageService", "Service")
                        .WithMany("ServiceRecords")
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("FK_ServiceRecords_LaguageServices")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Person");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.Gender", b =>
                {
                    b.Navigation("People");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.Image", b =>
                {
                    b.Navigation("LanguageServiceImages");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.LanguageService", b =>
                {
                    b.Navigation("LanguageServiceImages");

                    b.Navigation("ServiceRecords");
                });

            modelBuilder.Entity("DemoEx.Domain.Models.Person", b =>
                {
                    b.Navigation("ServiceRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
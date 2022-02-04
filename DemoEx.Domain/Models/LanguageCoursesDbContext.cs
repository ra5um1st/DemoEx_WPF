using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DemoEx.Domain.Models
{
    public partial class LanguageCoursesDbContext : DbContext
    {
        public LanguageCoursesDbContext()
        {
        }

        public LanguageCoursesDbContext(DbContextOptions<LanguageCoursesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<LanguageService> LaguageServices { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<ServiceRecord> ServiceRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=ngknn.ru;database=41P_Vaganov;user id=41П;password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.GenderName)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("Gender");
            });

            modelBuilder.Entity<LanguageService>(entity =>
            {
                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsFixedLength(true);

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(200)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.RegistarionDate).HasColumnType("date");

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Persons_Genders");
            });

            modelBuilder.Entity<ServiceRecord>(entity =>
            {
                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.ServiceRecords)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ServiceRecords_Persons");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceRecords)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ServiceRecords_LaguageServices");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

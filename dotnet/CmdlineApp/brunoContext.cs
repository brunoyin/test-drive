using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CmdlineApp
{
    public partial class brunoContext : DbContext
    {
        public static string ConnectionString {get; set;}
        public brunoContext()
        {
        }

        public brunoContext(DbContextOptions<brunoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<College> College { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                // optionsBuilder.UseNpgsql("Host=192.168.0.48;Database=bruno;Username=bruno;Password=bruno");
                optionsBuilder.UseNpgsql(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<College>(entity =>
            {
                entity.ToTable("college", "yin");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(12)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActAvg)
                    .HasColumnName("act_avg")
                    .HasColumnType("numeric");

                entity.Property(e => e.AdmRate)
                    .HasColumnName("adm_rate")
                    .HasColumnType("numeric");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(36);

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("numeric");

                entity.Property(e => e.Earnings)
                    .HasColumnName("earnings")
                    .HasColumnType("numeric");

                entity.Property(e => e.Enrollments)
                    .HasColumnName("enrollments")
                    .HasColumnType("numeric");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.Region).HasColumnName("region");

                entity.Property(e => e.SatAvg)
                    .HasColumnName("sat_avg")
                    .HasColumnType("numeric");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("character(2)");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasMaxLength(16);
            });
        }
    }
}

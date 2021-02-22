using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BatchTask.Models
{
    public partial class ELimgContext : DbContext
    {
        public ELimgContext()
        {
        }

        public ELimgContext(DbContextOptions<ELimgContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BatchTasks> BatchTasks { get; set; }
        public virtual DbSet<Elimg> Elimg { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<TblAdmin> TblAdmin { get; set; }
        public virtual DbSet<Awskeys> Awskeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(DBConnection.Connection_ELimg);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatchTasks>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LastEjecution).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Elimg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ELimg");

                entity.Property(e => e.Sno)
                    .HasColumnName("SNo")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.DateModified)
                    .HasColumnName("Date_Modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpload).HasColumnType("datetime");

                entity.Property(e => e.PalletNo).HasMaxLength(50);

                entity.Property(e => e.S3url)
                    .HasColumnName("S3URL")
                    .HasMaxLength(150);

                entity.Property(e => e.Sno)
                    .HasColumnName("SNo")
                    .HasMaxLength(50);

                entity.Property(e => e.UplodedToCloud).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Auto')");
            });

            modelBuilder.Entity<TblAdmin>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_admin");

                entity.Property(e => e.AdminPassword)
                    .HasColumnName("admin_password")
                    .HasMaxLength(50);

                entity.Property(e => e.AdminUser)
                    .HasColumnName("admin_user")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Awskeys>(entity =>
            {
                entity.ToTable("AWSkeys");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Pallet)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.S3url)
                    .IsRequired()
                    .HasColumnName("S3URL")
                    .HasMaxLength(150);

                entity.Property(e => e.SerialImage)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.YearKey)
                    .IsRequired()
                    .HasMaxLength(10);
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

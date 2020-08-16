using System;
using AnagramSolver.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AnagramSolver.EF.DatabaseFirst
{
    public partial class AnagramSolverDBFirstContext : DbContext
    {
        //private readonly string connectionString = "Server=LT-LIT-SC-0513;Database=AnagramSolver;" +
        //  "Integrated Security = true;Uid=auth_windows";

        public AnagramSolverDBFirstContext()
        {
        }

        public AnagramSolverDBFirstContext(DbContextOptions<AnagramSolverDBFirstContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CachedWordEntity> CachedWord { get; set; }
        public virtual DbSet<UserLogEntity> UserLog { get; set; }
        public virtual DbSet<WordEntity> Word { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Contracts.Settings.GetSettingsConnectionStringDBFirst());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CachedWordEntity>(entity =>
            {
                entity.Property(e => e.SearchWord).IsRequired();

                entity.HasOne(d => d.AnagramWord)
                    .WithMany(p => p.CachedWord)
                    .HasForeignKey(d => d.AnagramWordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CachedWor__Anagr__534D60F1");
            });

            modelBuilder.Entity<UserLogEntity>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.SearchTime).HasColumnType("datetime");

                entity.Property(e => e.UserAction)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserIp).IsRequired();

                //entity.HasOne(d => d.SearchWord)
                //    .WithMany()
                //    .HasForeignKey(d => d.SearchWordId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK__UserLog__SearchW__5535A963");
            });

            modelBuilder.Entity<WordEntity>(entity =>
            {
                entity.HasIndex(e => e.Word1)
                    .HasName("IX_Word");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Word1)
                    .IsRequired()
                    .HasColumnName("Word")
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    //public class AnagramSolverDBFirstContext : DbContext
    //{
    //    private readonly string connectionString = "Server=LT-LIT-SC-0513;Database=AnagramSolver;" +
    //        "Integrated Security = true;Uid=auth_windows";
    //    public virtual DbSet<WordEntity> Word { get; set; }
    //    public virtual DbSet<UserLogEntity> UserLog { get; set; }
    //    public virtual DbSet<CachedWordEntity> CachedWord { get; set; }

    //    public AnagramSolverDBFirstContext()
    //    {
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        if (!optionsBuilder.IsConfigured)
    //        {
    //            optionsBuilder.UseSqlServer(connectionString);
    //        }
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<WordEntity>(entity =>
    //        {
    //            entity.Property(e => e.Id)
    //            .HasColumnName("Id")
    //            .IsRequired();

    //            entity.Property(e => e.Word)
    //            .HasColumnName("Word")
    //            .HasColumnType("nvarchar")
    //            .HasMaxLength(255)
    //            .IsRequired();

    //            entity.Property(e => e.Category)
    //            .HasColumnName("Category")
    //            .HasColumnType("nvarchar")
    //            .HasMaxLength(255)
    //            .IsRequired();

    //            entity.HasKey(e => e.Id);

    //        });

    //        modelBuilder.Entity<UserLogEntity>(entity =>
    //        {
    //            entity.Property(e => e.UserIp)
    //            .HasColumnName("UserIp")
    //            .HasColumnType("nvarchar");

    //            entity.Property(e => e.SearchWordId)
    //            .HasColumnName("Category")
    //            .HasColumnType("int");

    //            entity.Property(e => e.SearchTime)
    //            .HasColumnName("SearchTime")
    //            .HasColumnType("datetime");

    //        });

    //        modelBuilder.Entity<CachedWordEntity>(entity =>
    //        {
    //            entity.Property(e => e.Id)
    //            .HasColumnName("Id")
    //            .IsRequired();

    //            entity.Property(e => e.SearchWord)
    //            .HasColumnName("SearchWord")
    //            .HasColumnType("nvarchar")
    //            .IsRequired();

    //            entity.Property(e => e.AnagramWordId)
    //            .HasColumnName("AnagramWordId")
    //            .HasColumnType("int")
    //            .IsRequired();

    //            entity.HasKey(e => e.Id);
    //            //entity.HasOne(d => d.WordModel)
    //            //.WithOne(p => p.CachedWord)
    //            //.HasForeignKey<CachedWord>(d => d.WordId);

    //        });
    //    }
    //}
}


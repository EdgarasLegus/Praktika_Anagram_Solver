using AnagramSolver.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AnagramSolver.EF.CodeFirst
{
    public class AnagramSolverCodeFirstContext : DbContext
    {
        //private readonly string connectionString = "Server=LT-LIT-SC-0513;Database=AnagramSolver_CodeFirst_New;" +
        //  "Integrated Security = true;Uid=auth_windows";

        public AnagramSolverCodeFirstContext()
        {
        }

        public AnagramSolverCodeFirstContext(DbContextOptions<AnagramSolverCodeFirstContext> options)
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
                optionsBuilder.UseSqlServer(Contracts.Settings.GetSettingsConnectionStringCodeFirst());
            }
        }
    }


}


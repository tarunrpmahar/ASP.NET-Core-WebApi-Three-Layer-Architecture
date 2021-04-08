using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Entities.Entities
{
    public partial class CommanderDBContext : DbContext
    {
        public CommanderDBContext()
        {
        }

        public CommanderDBContext(DbContextOptions<CommanderDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCommand> TblCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=CommanderConnectionString");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblCommand>(entity =>
            {
                entity.ToTable("tblCommands");

                entity.Property(e => e.HowTo)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Line).IsRequired();

                entity.Property(e => e.Platform).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

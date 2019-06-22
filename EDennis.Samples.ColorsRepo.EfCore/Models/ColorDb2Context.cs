using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EDennis.Samples.ColorsRepo.EfCore.Models {
    public partial class ColorDb2Context : DbContext {
        public ColorDb2Context() {
        }

        public ColorDb2Context(DbContextOptions<ColorDb2Context> options)
            : base(options) {
        }

        public virtual DbSet<Color> Colors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDb;Database=ColorDb2;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Color>(entity => {

                entity.ToTable("Color");

                entity.HasKey(e => e.Name)
                    .HasName("pkColor");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });
        }
    }
}

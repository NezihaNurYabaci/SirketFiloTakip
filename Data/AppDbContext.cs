using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Arac> Araclar { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<HasarKaydi> HasarKayitlari { get; set; }
        public DbSet<YakitKaydi> YakitKayitlari { get; set; }
        public DbSet<BakimKaydi> BakimKayitlari { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Arac>()
                .HasIndex(a => a.Plaka)
                .IsUnique();

            modelBuilder.Entity<BakimKaydi>()
                .Property(x => x.ToplamUcret)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HasarKaydi>()
                .Property(x => x.Maliyet)
                .HasPrecision(18, 2);

            modelBuilder.Entity<YakitKaydi>()
                .Property(x => x.Litre)
                .HasPrecision(18, 2);

            modelBuilder.Entity<YakitKaydi>()
                .Property(x => x.ToplamUcret)
                .HasPrecision(18, 2);
        }
    }
}


using KReca.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace KReca.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Urun> Urunler { get; set; }
    public DbSet<Kategori> Kategoriler { get; set; }
    public DbSet<Siparis> Siparisler { get; set; }
    public DbSet<SiparisDetay> SiparisDetaylar { get; set; }
    public DbSet<SiteAyarlari> SiteAyarlari { get; set; }
    public DbSet<IletisimMesaji> IletisimMesajlari { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Urun
        modelBuilder.Entity<Urun>(entity =>
        {
            entity.Property(e => e.Fiyat)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Kategori)
                  .WithMany(k => k.Urunler)
                  .HasForeignKey(e => e.KategoriId);
        });

        // Siparis
        modelBuilder.Entity<Siparis>(entity =>
        {
            entity.Property(e => e.ToplamTutar)
                  .HasColumnType("decimal(18,2)");
            entity.Property(e => e.KargoUcreti)
                  .HasColumnType("decimal(18,2)");
            entity.Property(e => e.GenelToplam)
                  .HasColumnType("decimal(18,2)");
        });

        // SiparisDetay
        modelBuilder.Entity<SiparisDetay>(entity =>
        {
            entity.Property(e => e.BirimFiyat)
                  .HasColumnType("decimal(18,2)");
            entity.Property(e => e.ToplamFiyat)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Siparis)
                  .WithMany(s => s.SiparisDetaylar)
                  .HasForeignKey(e => e.SiparisId);

            entity.HasOne(e => e.Urun)
                  .WithMany(u => u.SiparisDetaylar)
                  .HasForeignKey(e => e.UrunId);
        });

        // SiteAyarlari
        modelBuilder.Entity<SiteAyarlari>(entity =>
        {
            entity.Property(e => e.KargoUcreti)
                  .HasColumnType("decimal(18,2)");
            entity.Property(e => e.UcretsizKargoLimiti)
                  .HasColumnType("decimal(18,2)");
        });

        // Seed Data — başlangıç kategorileri
        modelBuilder.Entity<Kategori>().HasData(
            new Kategori { Id = 1, Ad = "Tıbbi Sülük", Aciklama = "Tıbbi amaçlı sülükler" },
            new Kategori { Id = 2, Ad = "Hacamat Malzemeleri", Aciklama = "Hacamat ekipmanları" }
        );

        // Seed Data — varsayılan site ayarları
        modelBuilder.Entity<SiteAyarlari>().HasData(
            new SiteAyarlari
            {
                Id = 1,
                KargoUcreti = 150,
                UcretsizKargoLimiti = 1000,
                UcretsizKargoAktif = false
            }
        );
    }
}
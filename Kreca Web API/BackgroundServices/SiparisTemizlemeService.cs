using KReca.Data;
using KReca.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace KRecaWebAPI.BackgroundServices;

public class SiparisTemizlemeService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SiparisTemizlemeService> _logger;

    public SiparisTemizlemeService(IServiceProvider serviceProvider, ILogger<SiparisTemizlemeService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Sipariş Temizleme Servisi başlatıldı. (Her 15 dakikada bir çalışacak)");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // 30 dakikadan eski ve Beklemede (0) olan siparişler
                var zamanSiniri = DateTime.UtcNow.AddMinutes(-30);
                
                var terkEdilenSiparisler = await context.Siparisler
                    .Include(s => s.SiparisDetaylar)
                    .ThenInclude(sd => sd.Urun)
                    .Where(s => s.Durum == SiparisDurumu.Beklemede && s.OlusturulmaTarihi < zamanSiniri)
                    .ToListAsync(stoppingToken);

                if (terkEdilenSiparisler.Any())
                {
                    foreach (var siparis in terkEdilenSiparisler)
                    {
                        // İptal durumuna çek (4)
                        siparis.Durum = SiparisDurumu.Iptal;
                        siparis.OdemeTamamlandi = false;

                        // Stokları geri yükle
                        foreach (var detay in siparis.SiparisDetaylar)
                        {
                            if (detay.Urun != null)
                            {
                                detay.Urun.StokAdedi += detay.Adet;
                            }
                        }

                        _logger.LogInformation("Terk edilen sipariş iptal edildi ve stokları geri yüklendi: #{SiparisId}", siparis.Id);
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş temizleme işlemi sırasında hata oluştu.");
            }

            // 15 dakikada bir kontrol et
            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }
}

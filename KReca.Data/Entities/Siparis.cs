using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace KReca.Data.Entities
    {
        public class Siparis
        {
            public int Id { get; set; }

            // Müşteri bilgileri
            public string AdSoyad { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Telefon { get; set; } = string.Empty;
            public string Adres { get; set; } = string.Empty;
            public string Sehir { get; set; } = string.Empty;
            public string PostaKodu { get; set; } = string.Empty;

            // Fatura bilgileri (Farklıysa)
            public string? FaturaAdresi { get; set; }
            public string? FaturaSehri { get; set; }
            public string? TcVeyaVergiNo { get; set; }
            public string? FirmaAdi { get; set; }

            // Sipariş bilgileri
            public decimal ToplamTutar { get; set; }
            public decimal KargoUcreti { get; set; } = 150;
            public decimal GenelToplam { get; set; }
            public SiparisDurumu Durum { get; set; } = SiparisDurumu.Beklemede;

            // PayTR bilgileri
            public string? PayTRMerchantOid { get; set; }
            public string? PayTROdemeId { get; set; }
            public bool OdemeTamamlandi { get; set; } = false;

            public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;

            // Navigation
            public ICollection<SiparisDetay> SiparisDetaylar { get; set; } = new List<SiparisDetay>();
        }

        public enum SiparisDurumu
        {
            Beklemede = 0,
            Onaylandi = 1,
            Kargoda = 2,
            TeslimEdildi = 3,
            Iptal = 4
        }
    }


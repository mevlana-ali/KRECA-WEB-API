using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KReca.Data.Entities
{
    public class Urun
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string? Aciklama { get; set; }
        public decimal Fiyat { get; set; }
        public int StokAdedi { get; set; }
        public string? ResimUrl { get; set; }
        public bool AktifMi { get; set; } = true;
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int KategoriId { get; set; }

        // Navigation
        public Kategori Kategori { get; set; } = null!;
        public ICollection<SiparisDetay> SiparisDetaylar { get; set; } = new List<SiparisDetay>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KReca.Data.Entities
{
   public class SiparisDetay
    {
        public int Id { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamFiyat { get; set; }

        // Foreign Keys
        public int SiparisId { get; set; }
        public int UrunId { get; set; }

        // Navigation
        public Siparis Siparis { get; set; } = null!;
        public Urun Urun { get; set; } = null!;
    }
}

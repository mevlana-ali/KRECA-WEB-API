using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KReca.Data.Entities
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string? Aciklama { get; set; }
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Urun> Urunler { get; set; } = new List<Urun>();
    }
}

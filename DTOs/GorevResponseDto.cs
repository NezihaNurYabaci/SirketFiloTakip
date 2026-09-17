using SirketFiloTakip.Models;

namespace SirketFiloTakip.DTOs
{
    public class GorevResponseDto
    {
        public int Id { get; set; }
        public string Amac { get; set; } = null!;
        public GorevDurumu Durum {  get; set; }
        public string KalkisYeri { get; set; } = null!;
        public string VarisYeri { get; set; } = null!;
        public DateTime PlanlananCikis { get; set; }
        public DateTime PlanlananDonus { get; set; }
        public DateTime? GercekCikis { get; set; }
        public DateTime? GercekDonus { get; set; }
        public int? BaslangicKm { get; set; }
        public int? BitisKm { get; set; }
        public int AracId { get; set; }
        public string AracPlaka { get; set; } = null!;
        public int CalisanId { get; set; }
        public string CalisanAdSoyad { get; set; } = null!;
    }
}

namespace SirketFiloTakip.Models
{
    public class Gorev
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public Arac Arac { get; set; } = null!;
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; } = null!;
        public string Amac { get; set; } = null!;
        public string KalkisYeri { get; set; } = null!;
        public string VarisYeri { get; set; } = null!;
        public DateTime PlanlananCikis { get; set; }
        public DateTime PlanlananDonus { get; set; }
        public DateTime? GercekCikis { get; set; }
        public DateTime? GercekDonus { get; set; }
        public int? BaslangicKm { get; set; }
        public int? BitisKm { get; set; }
        public GorevDurumu Durum {  get; set; }
        public ICollection<HasarKaydi> HasarKayitlari { get; set; } = new List<HasarKaydi>();
    }
}

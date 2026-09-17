namespace SirketFiloTakip.Models
{
    public class HasarKaydi
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public int? GorevId { get; set; }
        public string Durum { get; set; } = null!;
        public DateTime Tarih {  get; set; }
        public string Aciklama { get; set; } = null!;
        public decimal? Maliyet {  get; set; }
        public Arac Arac { get; set; } = null!;
        public Gorev? Gorev { get; set; }
    }
}

namespace SirketFiloTakip.Models
{
    public class BakimKaydi
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public DateTime Tarih {  get; set; }
        public string BakimTuru { get; set; } = null!;
        public decimal ToplamUcret {  get; set; }
        public int Kilometre {  get; set; }
        public string? Aciklama { get; set; }
        public int? SonrakiBakimKm { get; set; }
        public Arac Arac { get; set; } = null!;
    }
}

namespace SirketFiloTakip.Models
{
    public class YakitKaydi
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public Arac Arac { get; set; } = null!;
        public DateTime Tarih {  get; set; }
        public decimal Litre {  get; set; }
        public decimal ToplamUcret { get; set; }
        public int Kilometre { get; set; }
        public YakitTuru YakitTuru { get; set; }
    }
}

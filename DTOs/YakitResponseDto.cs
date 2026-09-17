using SirketFiloTakip.Models;

namespace SirketFiloTakip.DTOs
{
    public class YakitResponseDto
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public DateTime Tarih { get; set; }
        public decimal Litre { get; set; }
        public decimal ToplamUcret { get; set; }
        public int Kilometre { get; set; }
        public YakitTuru YakitTuru { get; set; }
        public string AracPlaka { get; set; } = null!;
    }
}

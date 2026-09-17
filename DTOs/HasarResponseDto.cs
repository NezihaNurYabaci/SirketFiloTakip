using SirketFiloTakip.Models;

namespace SirketFiloTakip.DTOs
{
    public class HasarResponseDto
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public int? GorevId { get; set; }
        public string Durum { get; set; } = null!;
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; } = null!;
        public decimal? Maliyet { get; set; }
        public string AracPlaka { get; set; } = null!;
        public string? GorevAmaci {  get; set; }

    }
}
       

using System.ComponentModel.DataAnnotations;

namespace SirketFiloTakip.DTOs
{
    public class HasarUpdateDto
    {
        [Range(1, int.MaxValue)]
        public int AracId { get; set; }

        [Range(1, int.MaxValue)]
        public int? GorevId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Durum { get; set; } = null!;
        public DateTime Tarih { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Aciklama { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal? Maliyet { get; set; }
    }
}

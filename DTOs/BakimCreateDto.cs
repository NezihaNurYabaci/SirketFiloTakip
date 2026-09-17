using System.ComponentModel.DataAnnotations;

namespace SirketFiloTakip.DTOs
{
    public class BakimCreateDto
    {
        [Range(1, int.MaxValue)]
        public int AracId { get; set; }
        public DateTime Tarih { get; set; }
        [Required]
        [StringLength(100)]
        public string BakimTuru { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal ToplamUcret { get; set; }

        [Range(0, int.MaxValue)]
        public int Kilometre { get; set; }
        public string? Aciklama { get; set; }

        [Range(0, int.MaxValue)]
        public int? SonrakiBakimKm { get; set; }
    }
}

 

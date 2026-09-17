using System.ComponentModel.DataAnnotations;

namespace SirketFiloTakip.DTOs
{
    public class GorevCreateDto
    {
        [Range(1, int.MaxValue)]
        public int AracId { get; set; }

        [Range(1, int.MaxValue)]
        public int CalisanId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3 )]
        public string Amac { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string KalkisYeri { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string VarisYeri { get; set; } = null!;

        public DateTime PlanlananCikis { get; set; }
        public DateTime PlanlananDonus { get; set; }
    }
}

using SirketFiloTakip.Models;
using System.ComponentModel.DataAnnotations;

namespace SirketFiloTakip.DTOs
{
    public class AracUpdateDto
    {
        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string Plaka { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Marka { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Model { get; set; } = null!;

        [Range(1990, 2100)]
        public int ModelYili { get; set; }

        [Range(0, int.MaxValue)]
        public int Kilometre { get; set; }
        public YakitTuru YakitTuru { get; set; }
        public AracTuru AracTuru { get; set; }
        public EhliyetSinifi GerekliEhliyetSinifi { get; set; }
    }
}

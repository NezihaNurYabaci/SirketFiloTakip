using SirketFiloTakip.Models;
using System.ComponentModel.DataAnnotations;

namespace SirketFiloTakip.DTOs

{
    public class CalisanCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string AdSoyad { get; set; } = null!;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Pozisyon { get; set; } = null!;
        [Required]
        [StringLength(15, MinimumLength = 10)]
        public string TelefonNo { get; set; } = null!;
        public EhliyetSinifi? Ehliyet { get; set; }
    }
}

using SirketFiloTakip.Models;

namespace SirketFiloTakip.DTOs
{
    public class CalisanResponseDto
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = null!;
        public string Pozisyon { get; set; } = null!;
        public string TelefonNo { get; set; } = null!;
        public EhliyetSinifi? Ehliyet { get; set; }
    }
}
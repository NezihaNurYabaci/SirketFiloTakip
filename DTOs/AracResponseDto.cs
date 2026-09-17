using SirketFiloTakip.Models;

namespace SirketFiloTakip.DTOs
{
    public class AracResponseDto
    {
        public int Id { get; set; }
        public string Plaka { get; set; } = null!;
        public string Marka { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int ModelYili { get; set; }
        public int Kilometre { get; set; }
        public YakitTuru YakitTuru { get; set; }
        public AracDurumu Durum { get; set; }
        public AracTuru AracTuru { get; set; }
        public EhliyetSinifi GerekliEhliyetSinifi { get; set; }
    }
}
namespace SirketFiloTakip.DTOs
{
    public class AracYakitMaliyetResponseDto
    {
        public int AracId { get; set; }
        public string Plaka { get; set; } = null!;
        public decimal ToplamYakitMaliyeti { get; set; }
    }
}

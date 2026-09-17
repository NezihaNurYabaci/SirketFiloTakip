namespace SirketFiloTakip.DTOs
{
    public class AracBakimMaliyetResponseDto
    {
        public int AracId { get; set; }
        public string Plaka { get; set; } = null!;
        public decimal ToplamBakimMaliyeti { get; set; }
    }
}

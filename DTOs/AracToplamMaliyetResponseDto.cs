namespace SirketFiloTakip.DTOs
{
    public class AracToplamMaliyetResponseDto
    {
        public int AracId { get; set; }
        public string Plaka { get; set; } = null!;
        public decimal YakitMaliyeti { get; set; }
        public decimal BakimMaliyeti { get; set; }
        public decimal HasarMaliyeti { get; set; }
        public decimal ToplamMaliyet {  get; set; }
    }
}

namespace SirketFiloTakip.DTOs
{
    public class CalisanGorevSayisiResponseDto
    {
        public int CalisanId { get; set; }
        public string CalisanAdSoyad { get; set; } = null!;
        public int GorevSayisi {  get; set; }
    }
}

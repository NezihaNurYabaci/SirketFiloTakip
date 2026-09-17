namespace SirketFiloTakip.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = null!;
        public string Pozisyon { get; set; } = null!;
        public string TelefonNo { get; set; } = null!;
        public EhliyetSinifi? Ehliyet { get; set; }
        public ICollection<Gorev> Gorevler { get; set; } = new List<Gorev>();
    }
}

namespace SirketFiloTakip.Models
{
    public class Arac
    {
        public int Id { get; set; }
        public string Plaka { get; set; } = null!;
        public string Marka { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int ModelYili { get; set; }
        public int Kilometre { get; set; }
        public YakitTuru YakitTuru { get; set; } 
        public AracDurumu Durum { get; set; } = AracDurumu.Musait;
        public AracTuru AracTuru { get; set; }
        public EhliyetSinifi GerekliEhliyetSinifi { get; set; }
        public ICollection<YakitKaydi> YakitKayitlari { get; set; } = new List<YakitKaydi>();
        public ICollection<BakimKaydi> BakimKayitlari { get; set; } = new List<BakimKaydi>();
        public ICollection<HasarKaydi> HasarKayitlari { get; set; } = new List<HasarKaydi>();
        public ICollection<Gorev> Gorevler { get; set; } = new List<Gorev>();

    }
}

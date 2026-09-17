using SirketFiloTakip.Models;
using System.ComponentModel.DataAnnotations;

namespace SirketFiloTakip.DTOs
{
    public class YakitUpdateDto
    {
        [Range(1, int.MaxValue)]
        public int AracId { get; set; }
        public DateTime Tarih { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Litre { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ToplamUcret { get; set; }

        [Range(0, int.MaxValue)]
        public int Kilometre { get; set; }
        public YakitTuru YakitTuru { get; set; }
    }
}

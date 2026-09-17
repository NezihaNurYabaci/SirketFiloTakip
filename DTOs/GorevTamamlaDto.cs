using System.ComponentModel.DataAnnotations;

namespace SirketFiloTakip.DTOs
{

    public class GorevTamamlaDto
    {
        [Range(0, int.MaxValue)]
        public int BitisKm { get; set; }
    }
}

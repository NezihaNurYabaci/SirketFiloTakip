using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Data;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaporController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RaporController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("musait-araclar")]
        public async Task<ActionResult<List<AracResponseDto>>> MusaitAraclar()
        {
            var musaitAraclar = await _context.Araclar
                .Where(a => a.Durum == AracDurumu.Musait)
                .Select(a => new AracResponseDto
                {
                    Id = a.Id,
                    Plaka = a.Plaka,
                    Marka = a.Marka,
                    Model = a.Model,
                    ModelYili = a.ModelYili,
                    Kilometre = a.Kilometre,
                    YakitTuru = a.YakitTuru,
                    Durum = a.Durum,
                    AracTuru = a.AracTuru,
                    GerekliEhliyetSinifi = a.GerekliEhliyetSinifi
                })
                .ToListAsync();

            return Ok(musaitAraclar);
        }

        [HttpGet("araclar-kilometre")]
        public async Task<ActionResult<List<AracResponseDto>>> AraclarKilometre()
        {
            var araclar = await _context.Araclar
                .OrderByDescending(a => a.Kilometre)
                .Select(a => new AracResponseDto
                {
                    Id = a.Id,
                    Plaka = a.Plaka,
                    Marka = a.Marka,
                    Model = a.Model,
                    ModelYili = a.ModelYili,
                    Kilometre = a.Kilometre,
                    YakitTuru = a.YakitTuru,
                    Durum = a.Durum,
                    AracTuru = a.AracTuru,
                    GerekliEhliyetSinifi = a.GerekliEhliyetSinifi
                })
                .ToListAsync();

            return Ok(araclar);
        }

        [HttpGet("arac-yakit-maliyeti/{aracId}")]
        public async Task<ActionResult<AracYakitMaliyetResponseDto>> AracYakitMaliyeti(int aracId)
        {
            var arac = await _context.Araclar.FindAsync(aracId);

            if (arac == null)
            {
                return NotFound();
            }

            var aracYakitMaliyeti = await _context.YakitKayitlari
                .Where(y => y.AracId == aracId)
                .SumAsync(y => y.ToplamUcret);

            var response = new AracYakitMaliyetResponseDto
            {
                AracId = aracId,
                Plaka = arac.Plaka,
                ToplamYakitMaliyeti = aracYakitMaliyeti
            };

            return Ok(response);
        }

        [HttpGet("arac-bakim-maliyeti/{aracId}")]
        public async Task<ActionResult<AracBakimMaliyetResponseDto>> AracBakimMaliyeti(int aracId)
        {
            var arac = await _context.Araclar.FindAsync(aracId);

            if (arac == null)
            {
                return NotFound();
            }

            var aracBakimMaliyeti = await _context.BakimKayitlari
                .Where(b => b.AracId == aracId)
                .SumAsync(b => b.ToplamUcret);

            var response = new AracBakimMaliyetResponseDto
            {
                AracId = aracId,
                Plaka = arac.Plaka,
                ToplamBakimMaliyeti = aracBakimMaliyeti
            };

            return Ok(response);
        }

        [HttpGet("calisan-gorev-sayilari")]
        public async Task<ActionResult<List<CalisanGorevSayisiResponseDto>>> CalisanGorevSayilari()
        {
            var sonuc = await _context.Gorevler
                .GroupBy(g => new
                {
                    g.CalisanId,
                    g.Calisan.AdSoyad
                })
                .Select(g => new CalisanGorevSayisiResponseDto
                {
                    CalisanId = g.Key.CalisanId,
                    CalisanAdSoyad = g.Key.AdSoyad,
                    GorevSayisi = g.Count()
                })
                .ToListAsync();

            return Ok(sonuc);
        }

        [HttpGet("arac-toplam-maliyet/{aracId}")]
        public async Task<ActionResult<AracToplamMaliyetResponseDto>> AracToplamMaliyet(int aracId)
        {
            var arac = await _context.Araclar.FindAsync(aracId);

            if (arac == null)
            {
                return NotFound();
            }

            var yakitMaliyeti = await _context.YakitKayitlari
                .Where(y => y.AracId == aracId)
                .SumAsync(y => y.ToplamUcret);

            var bakimMaliyeti = await _context.BakimKayitlari
                .Where(b => b.AracId == aracId)
                .SumAsync(b => b.ToplamUcret);

            var hasarMaliyeti = await _context.HasarKayitlari
                .Where(h => h.AracId == aracId)
                .SumAsync(h => h.Maliyet) ?? 0;

            var toplamMaliyet =
                yakitMaliyeti +
                bakimMaliyeti +
                hasarMaliyeti;

            var response = new AracToplamMaliyetResponseDto
            {
                AracId = aracId,
                Plaka = arac.Plaka,
                YakitMaliyeti = yakitMaliyeti,
                BakimMaliyeti = bakimMaliyeti,
                HasarMaliyeti = hasarMaliyeti,
                ToplamMaliyet = toplamMaliyet
            };

            return Ok(response);
        }
    }
}
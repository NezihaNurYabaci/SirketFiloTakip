using Microsoft.AspNetCore.Mvc;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Services;

namespace SirketFiloTakip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BakimController : ControllerBase
    {
        private readonly BakimService _bakimService;

        public BakimController(BakimService bakimService)
        {
            _bakimService = bakimService;
        }

        [HttpPost]
        public async Task<ActionResult> BakimEkle(BakimCreateDto dto)
        {
            var sonuc = await _bakimService.BakimEkle(dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }

        [HttpGet]
        public async Task<ActionResult<List<BakimResponseDto>>> GetBakimKayitlari()
        {
            var sonuc = await _bakimService.GetBakimKayitlari();
            return Ok(sonuc);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BakimResponseDto>> GetBakimKaydi(int id)
        {
            var sonuc = await _bakimService.GetBakimKaydi(id);

            if (sonuc == null)
            {
                return NotFound();
            }

            return Ok(sonuc);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> BakimKaydiSil(int id)
        {
            var sonuc = await _bakimService.BakimKaydiSil(id);

            if (!sonuc)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> BakimKaydiGuncelle(int id, BakimUpdateDto dto)
        {
            var sonuc = await _bakimService.BakimKaydiGuncelle(id, dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }
    }
}
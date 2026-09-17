
using Microsoft.AspNetCore.Mvc;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Services;

namespace SirketFiloTakip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YakitController : ControllerBase
    {
        private readonly YakitService _yakitService;

        public YakitController(YakitService yakitService)
        {
            _yakitService = yakitService;
        }

        [HttpPost]
        public async Task<ActionResult> YakitEkle(YakitCreateDto dto)
        {
            var sonuc = await _yakitService.YakitEkle(dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }

        [HttpGet]
        public async Task<ActionResult<List<YakitResponseDto>>> GetYakitKayitlari()
        {
            var sonuc = await _yakitService.GetYakitKayitlari();
            return Ok(sonuc);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<YakitResponseDto>> GetYakitKaydi(int id)
        {
            var sonuc = await _yakitService.GetYakitKaydi(id);

            if (sonuc == null)
            {
                return NotFound();
            }

            return Ok(sonuc);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> YakitKaydiSil(int id)
        {
            var sonuc = await _yakitService.YakitKaydiSil(id);

            if (!sonuc)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> YakitKaydiGuncelle(int id, YakitUpdateDto dto)
        {
            var sonuc = await _yakitService.YakitKaydiGuncelle(id, dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }
    }
}
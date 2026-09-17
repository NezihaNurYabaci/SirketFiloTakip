using Microsoft.AspNetCore.Mvc;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Services;

namespace SirketFiloTakip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalisanController : ControllerBase
    {
        private readonly CalisanService _calisanService;

        public CalisanController(CalisanService calisanService)
        {
            _calisanService = calisanService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CalisanResponseDto>>> GetCalisanlar()
        {
            var calisanlar = await _calisanService.GetCalisanlar();
            return Ok(calisanlar);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalisanResponseDto>> GetCalisan(int id)
        {
            var calisan = await _calisanService.GetCalisan(id);

            if (calisan == null)
            {
                return NotFound();
            }

            return Ok(calisan);
        }

        [HttpPost]
        public async Task<ActionResult<CalisanResponseDto>> CalisanEkle(CalisanCreateDto dto)
        {
            var calisan = await _calisanService.CalisanEkle(dto);
            return Ok(calisan);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CalisanResponseDto>> CalisanGuncelle(
            int id,
            CalisanUpdateDto dto)
        {
            var calisan = await _calisanService.CalisanGuncelle(id, dto);

            if (!calisan.Success)
            {
                return BadRequest(calisan.ErrorMessage);
            }

            return Ok(calisan.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CalisanSil(int id)
        {
            var sonuc = await _calisanService.CalisanSil(id);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Services;

namespace SirketFiloTakip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AracController : ControllerBase
    {
        private readonly AracService _aracService;

        public AracController(AracService aracService)
        {
            _aracService = aracService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AracResponseDto>>> GetAraclar()
        {
            var sonuc = await _aracService.GetAraclar();
            return Ok(sonuc);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AracResponseDto>> GetArac(int id)
        {
            var sonuc = await _aracService.GetArac(id);

            if (sonuc == null)
            {
                return NotFound();
            }

            return Ok(sonuc);
        }

        [HttpPost]
        public async Task<ActionResult<AracResponseDto>> AracEkle(AracCreateDto dto)
        {
            var sonuc = await _aracService.AracEkle(dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AracResponseDto>> AracGuncelle(int id, AracUpdateDto dto)
        {
            var sonuc = await _aracService.AracGuncelle(id, dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AracResponseDto>> AracSil(int id)
        {
            var sonuc = await _aracService.AracSil(id);

            if (!sonuc)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Services;

namespace SirketFiloTakip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HasarController : ControllerBase
    {
        private readonly HasarService _hasarService;

        public HasarController(HasarService hasarService)
        {
            _hasarService = hasarService;
        }

        [HttpPost]
        public async Task<ActionResult> HasarKaydiEkle(HasarCreateDto dto)
        {
            var sonuc = await _hasarService.HasarEkle(dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }

        [HttpGet]
        public async Task<ActionResult<List<HasarResponseDto>>> GetHasarKayitlari()
        {
            var sonuc = await _hasarService.GetHasarKayitlari();
            return Ok(sonuc);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HasarResponseDto>> GetHasarKaydi(int id)
        {
            var sonuc = await _hasarService.GetHasarKaydi(id);

            if (sonuc == null)
            {
                return NotFound();
            }

            return Ok(sonuc);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> HasarKaydiSil(int id)
        {
            var sonuc = await _hasarService.HasarKaydiSil(id);

            if (!sonuc)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> HasarKaydiGuncelle(
            int id,
            HasarUpdateDto dto)
        {
            var sonuc = await _hasarService.HasarKaydiGuncelle(id, dto);

            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }

            return Ok(sonuc.Data);
        }
    }
}
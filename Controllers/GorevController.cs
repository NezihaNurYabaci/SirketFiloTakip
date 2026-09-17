using Microsoft.AspNetCore.Mvc;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Services;

namespace SirketFiloTakip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GorevController : ControllerBase
    {
        private readonly GorevService _gorevService;

        public GorevController(GorevService gorevService)
        {
            _gorevService = gorevService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GorevResponseDto>>> GetGorevler()
        {
            var sonuc = await _gorevService.GetGorevler();
            return Ok(sonuc);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GorevResponseDto>> GetGorev(int id)
        {
            var gorev = await _gorevService.GetGorev(id);
            if(gorev == null)
            {
                return NotFound();
            }
            return Ok(gorev);
        }

        [HttpPost]
        public async Task<ActionResult<GorevResponseDto>> GorevEkle(GorevCreateDto dto)
        {
           var sonuc = await _gorevService.GorevEkle(dto);
            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }
            return Ok(sonuc.Data);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> GorevGuncelle(int id, GorevUpdateDto dto)
        {
            var gorev = await _gorevService.GorevGuncelle(id, dto);
            if (!gorev.Success)
            {
                return BadRequest(gorev.ErrorMessage);
            }
            return Ok(gorev.Data);
        }

        [HttpPut("{id}/baslat")]
        public async Task<ActionResult> GorevBaslat(int id)
        {
            var gorev = await _gorevService.GorevBaslat(id);
            if (!gorev.Success)
            {
                return BadRequest(gorev.ErrorMessage);
            }
            return Ok(gorev.Data);
        }

        [HttpPut("{id}/tamamla")]
        public async Task<ActionResult> GorevTamamla(int id, GorevTamamlaDto dto)
        {
            var sonuc = await _gorevService.GorevTamamla(id, dto);
            if (!sonuc.Success)
            {
                return BadRequest(sonuc.ErrorMessage);
            }
            return Ok(sonuc.Data);
        }
   
        [HttpDelete("{id}")]
        public async Task<ActionResult> GorevSil(int id)
        {
            var sonuc = await _gorevService.GorevSil(id);
            if (!sonuc)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
       
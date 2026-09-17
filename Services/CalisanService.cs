using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Data;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Services
{
    public class CalisanService
    {
        private readonly AppDbContext _context;
        public CalisanService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CalisanResponseDto>> GetCalisanlar()
        {
            var calisanlar = await _context.Calisanlar.ToListAsync();
            var response = calisanlar.Select(c => new CalisanResponseDto
            {
                Id = c.Id,
                AdSoyad = c.AdSoyad,
                Pozisyon = c.Pozisyon,
                TelefonNo = c.TelefonNo,
                Ehliyet = c.Ehliyet
            }).ToList();
            return response;
        }
        public async Task<CalisanResponseDto?> GetCalisan(int id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan == null)
            {
                return null;
            }
            var response = new CalisanResponseDto
            {
                Id = calisan.Id,
                AdSoyad = calisan.AdSoyad,
                Pozisyon = calisan.Pozisyon,
                TelefonNo = calisan.TelefonNo,
                Ehliyet = calisan.Ehliyet
            };
            return response;
        }
        public async Task<CalisanResponseDto> CalisanEkle(CalisanCreateDto dto)
        {
            var calisan = new Calisan
            {
                AdSoyad = dto.AdSoyad,
                Pozisyon = dto.Pozisyon,
                TelefonNo = dto.TelefonNo,
                Ehliyet = dto.Ehliyet
            };
            _context.Calisanlar.Add(calisan);
            await _context.SaveChangesAsync();
            var response = new CalisanResponseDto
            {
                Id = calisan.Id,
                AdSoyad = calisan.AdSoyad,
                Pozisyon = calisan.Pozisyon,
                TelefonNo = calisan.TelefonNo,
                Ehliyet = calisan.Ehliyet
            };
            return response;
        }
        public async Task<ServiceResult<CalisanResponseDto>> CalisanGuncelle(int id, CalisanUpdateDto dto)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);
            if(calisan == null)
            {
                return new ServiceResult<CalisanResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Çalışan kaydı bulunamadı."
                };
            }
            calisan.AdSoyad = dto.AdSoyad;
            calisan.Pozisyon = dto.Pozisyon;
            calisan.TelefonNo = dto.TelefonNo;
            calisan.Ehliyet = dto.Ehliyet;
            await _context.SaveChangesAsync();

            var response = new CalisanResponseDto
            {
                Id=calisan.Id,
                AdSoyad = calisan.AdSoyad,
                Pozisyon = calisan.Pozisyon,
                TelefonNo = calisan.TelefonNo,
                Ehliyet = calisan.Ehliyet
            };
            return new ServiceResult<CalisanResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<ServiceResult<bool>> CalisanSil(int id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);

            if (calisan == null)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    Data = false,
                    ErrorMessage = "Çalışan kaydı bulunamadı."
                };
            }

            var goreviVarMi = await _context.Gorevler
                .AnyAsync(g => g.CalisanId == id);

            if (goreviVarMi)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    Data = false,
                    ErrorMessage = "Bu çalışana ait görev kayıtları bulunduğu için çalışan silinemez."
                };
            }

            _context.Calisanlar.Remove(calisan);
            await _context.SaveChangesAsync();

            return new ServiceResult<bool>
            {
                Success = true,
                Data = true,
                ErrorMessage = null
            };
        }
    }
}

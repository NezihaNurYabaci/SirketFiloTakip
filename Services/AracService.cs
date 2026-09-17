using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Data;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Services
{
    public class AracService
    {
        private readonly AppDbContext _context;

        public AracService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AracResponseDto>> GetAraclar()
        {
            var araclar = await _context.Araclar.ToListAsync();

            var response = araclar.Select(a => new AracResponseDto
            {
                Id = a.Id,
                Plaka = a.Plaka,
                Marka = a.Marka,
                Model = a.Model,
                ModelYili = a.ModelYili,
                Kilometre = a.Kilometre,
                AracTuru = a.AracTuru,
                GerekliEhliyetSinifi = a.GerekliEhliyetSinifi,
                YakitTuru = a.YakitTuru,
                Durum = a.Durum
            }).ToList();

            return response;
        }

        public async Task<AracResponseDto?> GetArac(int id)
        {
            var arac = await _context.Araclar.FindAsync(id);

            if (arac == null)
            {
                return null;
            }

            var response = new AracResponseDto
            {
                Id = arac.Id,
                Plaka = arac.Plaka,
                Marka = arac.Marka,
                Model = arac.Model,
                ModelYili = arac.ModelYili,
                Kilometre = arac.Kilometre,
                AracTuru = arac.AracTuru,
                GerekliEhliyetSinifi = arac.GerekliEhliyetSinifi,
                YakitTuru = arac.YakitTuru,
                Durum = arac.Durum
            };

            return response;
        }

        public async Task<ServiceResult<AracResponseDto>> AracEkle(AracCreateDto dto)
        {
            var plakaVarMi = await _context.Araclar
                .AnyAsync(a => a.Plaka == dto.Plaka);

            if (plakaVarMi)
            {
                return new ServiceResult<AracResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Bu plakaya sahip bir araç zaten kayıtlı."
                };
            }

            var arac = new Arac
            {
                Plaka = dto.Plaka,
                Marka = dto.Marka,
                Model = dto.Model,
                ModelYili = dto.ModelYili,
                Kilometre = dto.Kilometre,
                AracTuru = dto.AracTuru,
                GerekliEhliyetSinifi = dto.GerekliEhliyetSinifi,
                YakitTuru = dto.YakitTuru,
                Durum = AracDurumu.Musait
            };

            _context.Araclar.Add(arac);
            await _context.SaveChangesAsync();

            var response = new AracResponseDto
            {
                Id = arac.Id,
                Plaka = arac.Plaka,
                Marka = arac.Marka,
                Model = arac.Model,
                ModelYili = arac.ModelYili,
                Kilometre = arac.Kilometre,
                AracTuru = arac.AracTuru,
                GerekliEhliyetSinifi = arac.GerekliEhliyetSinifi,
                YakitTuru = arac.YakitTuru,
                Durum = arac.Durum
            };

            return new ServiceResult<AracResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<ServiceResult<AracResponseDto>> AracGuncelle(
            int id,
            AracUpdateDto dto)
        {
            var arac = await _context.Araclar.FindAsync(id);

            if (arac == null)
            {
                return new ServiceResult<AracResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            var plakaVarMi = await _context.Araclar
                .AnyAsync(a => a.Plaka == dto.Plaka && a.Id != id);

            if (plakaVarMi)
            {
                return new ServiceResult<AracResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Bu plakaya sahip başka bir araç zaten kayıtlı."
                };
            }

            if (dto.Kilometre < arac.Kilometre)
            {
                return new ServiceResult<AracResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kilometresi mevcut kilometreden küçük olamaz."
                };
            }

            arac.Plaka = dto.Plaka;
            arac.Marka = dto.Marka;
            arac.Model = dto.Model;
            arac.ModelYili = dto.ModelYili;
            arac.Kilometre = dto.Kilometre;
            arac.AracTuru = dto.AracTuru;
            arac.GerekliEhliyetSinifi = dto.GerekliEhliyetSinifi;
            arac.YakitTuru = dto.YakitTuru;

            await _context.SaveChangesAsync();

            var response = new AracResponseDto
            {
                Id = arac.Id,
                Plaka = arac.Plaka,
                Marka = arac.Marka,
                Model = arac.Model,
                ModelYili = arac.ModelYili,
                Kilometre = arac.Kilometre,
                AracTuru = arac.AracTuru,
                GerekliEhliyetSinifi = arac.GerekliEhliyetSinifi,
                YakitTuru = arac.YakitTuru,
                Durum = arac.Durum
            };

            return new ServiceResult<AracResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<bool> AracSil(int id)
        {
            var arac = await _context.Araclar.FindAsync(id);

            if (arac == null)
            {
                return false;
            }

            _context.Araclar.Remove(arac);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
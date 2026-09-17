using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Data;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Services
{
    public class YakitService
    {
        private readonly AppDbContext _context;

        public YakitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<YakitResponseDto>> YakitEkle(YakitCreateDto dto)
        {
            var arac = await _context.Araclar.FindAsync(dto.AracId);

            if (arac == null)
            {
                return new ServiceResult<YakitResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            if (dto.Kilometre < arac.Kilometre)
            {
                return new ServiceResult<YakitResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Yakıt kilometresi aracın mevcut kilometresinden küçük olamaz."
                };
            }

            arac.Kilometre = dto.Kilometre;

            var yakitKaydi = new YakitKaydi
            {
                AracId = dto.AracId,
                Tarih = dto.Tarih,
                Litre = dto.Litre,
                ToplamUcret = dto.ToplamUcret,
                Kilometre = dto.Kilometre,
                YakitTuru = dto.YakitTuru
            };

            _context.YakitKayitlari.Add(yakitKaydi);
            await _context.SaveChangesAsync();

            var response = new YakitResponseDto
            {
                Id = yakitKaydi.Id,
                AracId = yakitKaydi.AracId,
                Tarih = yakitKaydi.Tarih,
                Litre = yakitKaydi.Litre,
                ToplamUcret = yakitKaydi.ToplamUcret,
                Kilometre = yakitKaydi.Kilometre,
                YakitTuru = yakitKaydi.YakitTuru,
                AracPlaka = arac.Plaka
            };

            return new ServiceResult<YakitResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<List<YakitResponseDto>> GetYakitKayitlari()
        {
            var yakitKayitlari = await _context.YakitKayitlari
                .Include(y => y.Arac)
                .ToListAsync();

            var response = yakitKayitlari.Select(y => new YakitResponseDto
            {
                Id = y.Id,
                AracId = y.AracId,
                Tarih = y.Tarih,
                ToplamUcret = y.ToplamUcret,
                Litre = y.Litre,
                Kilometre = y.Kilometre,
                YakitTuru = y.YakitTuru,
                AracPlaka = y.Arac.Plaka
            }).ToList();

            return response;
        }

        public async Task<YakitResponseDto?> GetYakitKaydi(int id)
        {
            var yakitKaydi = await _context.YakitKayitlari
                .Include(y => y.Arac)
                .FirstOrDefaultAsync(y => y.Id == id);

            if (yakitKaydi == null)
            {
                return null;
            }

            var response = new YakitResponseDto
            {
                Id = yakitKaydi.Id,
                AracId = yakitKaydi.AracId,
                Tarih = yakitKaydi.Tarih,
                ToplamUcret = yakitKaydi.ToplamUcret,
                Litre = yakitKaydi.Litre,
                Kilometre = yakitKaydi.Kilometre,
                YakitTuru = yakitKaydi.YakitTuru,
                AracPlaka = yakitKaydi.Arac.Plaka
            };

            return response;
        }

        public async Task<bool> YakitKaydiSil(int id)
        {
            var yakitKaydi = await _context.YakitKayitlari.FindAsync(id);

            if (yakitKaydi == null)
            {
                return false;
            }

            _context.YakitKayitlari.Remove(yakitKaydi);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ServiceResult<YakitResponseDto>> YakitKaydiGuncelle(
            int id,
            YakitUpdateDto dto)
        {
            var yakitKaydi = await _context.YakitKayitlari.FindAsync(id);

            if (yakitKaydi == null)
            {
                return new ServiceResult<YakitResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Yakıt kaydı bulunamadı."
                };
            }

            var arac = await _context.Araclar.FindAsync(dto.AracId);

            if (arac == null)
            {
                return new ServiceResult<YakitResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            if (dto.Kilometre < yakitKaydi.Kilometre)
            {
                return new ServiceResult<YakitResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Yeni kilometre eski kilometreden küçük olamaz."
                };
            }

            if (dto.Kilometre > arac.Kilometre)
            {
                arac.Kilometre = dto.Kilometre;
            }

            yakitKaydi.AracId = dto.AracId;
            yakitKaydi.Tarih = dto.Tarih;
            yakitKaydi.Litre = dto.Litre;
            yakitKaydi.ToplamUcret = dto.ToplamUcret;
            yakitKaydi.Kilometre = dto.Kilometre;
            yakitKaydi.YakitTuru = dto.YakitTuru;

            await _context.SaveChangesAsync();

            var response = new YakitResponseDto
            {
                Id = yakitKaydi.Id,
                AracId = yakitKaydi.AracId,
                Tarih = yakitKaydi.Tarih,
                Litre = yakitKaydi.Litre,
                ToplamUcret = yakitKaydi.ToplamUcret,
                Kilometre = yakitKaydi.Kilometre,
                YakitTuru = yakitKaydi.YakitTuru,
                AracPlaka = arac.Plaka
            };

            return new ServiceResult<YakitResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Data;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Services
{
    public class BakimService
    {
        private readonly AppDbContext _context;

        public BakimService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<BakimResponseDto>> BakimEkle(BakimCreateDto dto)
        {
            var arac = await _context.Araclar.FindAsync(dto.AracId);

            if (arac == null)
            {
                return new ServiceResult<BakimResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            if (dto.Kilometre < arac.Kilometre)
            {
                return new ServiceResult<BakimResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Bakım kilometresi aracın mevcut kilometresinden küçük olamaz."
                };
            }

            arac.Kilometre = dto.Kilometre;

            var bakimKaydi = new BakimKaydi
            {
                AracId = dto.AracId,
                Tarih = dto.Tarih,
                BakimTuru = dto.BakimTuru,
                ToplamUcret = dto.ToplamUcret,
                Kilometre = dto.Kilometre,
                Aciklama = dto.Aciklama,
                SonrakiBakimKm = dto.SonrakiBakimKm
            };

            _context.BakimKayitlari.Add(bakimKaydi);
            await _context.SaveChangesAsync();

            var response = new BakimResponseDto
            {
                Id = bakimKaydi.Id,
                AracId = bakimKaydi.AracId,
                Tarih = bakimKaydi.Tarih,
                ToplamUcret = bakimKaydi.ToplamUcret,
                Kilometre = bakimKaydi.Kilometre,
                Aciklama = bakimKaydi.Aciklama,
                SonrakiBakimKm = bakimKaydi.SonrakiBakimKm,
                AracPlaka = arac.Plaka,
                BakimTuru = bakimKaydi.BakimTuru
            };

            return new ServiceResult<BakimResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<List<BakimResponseDto>> GetBakimKayitlari()
        {
            var bakimKayitlari = await _context.BakimKayitlari
                .Include(b => b.Arac)
                .ToListAsync();

            var response = bakimKayitlari.Select(b => new BakimResponseDto
            {
                Id = b.Id,
                AracId = b.AracId,
                Tarih = b.Tarih,
                ToplamUcret = b.ToplamUcret,
                Kilometre = b.Kilometre,
                Aciklama = b.Aciklama,
                SonrakiBakimKm = b.SonrakiBakimKm,
                AracPlaka = b.Arac.Plaka,
                BakimTuru = b.BakimTuru
            }).ToList();

            return response;
        }

        public async Task<BakimResponseDto?> GetBakimKaydi(int id)
        {
            var bakimKaydi = await _context.BakimKayitlari
                .Include(b => b.Arac)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bakimKaydi == null)
            {
                return null;
            }

            var response = new BakimResponseDto
            {
                Id = bakimKaydi.Id,
                AracId = bakimKaydi.AracId,
                Tarih = bakimKaydi.Tarih,
                ToplamUcret = bakimKaydi.ToplamUcret,
                Aciklama = bakimKaydi.Aciklama,
                Kilometre = bakimKaydi.Kilometre,
                SonrakiBakimKm = bakimKaydi.SonrakiBakimKm,
                AracPlaka = bakimKaydi.Arac.Plaka,
                BakimTuru = bakimKaydi.BakimTuru
            };

            return response;
        }

        public async Task<bool> BakimKaydiSil(int id)
        {
            var bakimKaydi = await _context.BakimKayitlari.FindAsync(id);

            if (bakimKaydi == null)
            {
                return false;
            }

            _context.BakimKayitlari.Remove(bakimKaydi);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ServiceResult<BakimResponseDto>> BakimKaydiGuncelle(
            int id,
            BakimUpdateDto dto)
        {
            var bakimKaydi = await _context.BakimKayitlari.FindAsync(id);

            if (bakimKaydi == null)
            {
                return new ServiceResult<BakimResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Bakım kaydı bulunamadı."
                };
            }

            var arac = await _context.Araclar.FindAsync(dto.AracId);

            if (arac == null)
            {
                return new ServiceResult<BakimResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            if (dto.Kilometre < bakimKaydi.Kilometre)
            {
                return new ServiceResult<BakimResponseDto>
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

            bakimKaydi.AracId = dto.AracId;
            bakimKaydi.ToplamUcret = dto.ToplamUcret;
            bakimKaydi.Aciklama = dto.Aciklama;
            bakimKaydi.Kilometre = dto.Kilometre;
            bakimKaydi.Tarih = dto.Tarih;
            bakimKaydi.SonrakiBakimKm = dto.SonrakiBakimKm;
            bakimKaydi.BakimTuru = dto.BakimTuru;

            await _context.SaveChangesAsync();

            var response = new BakimResponseDto
            {
                Id = bakimKaydi.Id,
                AracId = bakimKaydi.AracId,
                ToplamUcret = bakimKaydi.ToplamUcret,
                Aciklama = bakimKaydi.Aciklama,
                Tarih = bakimKaydi.Tarih,
                Kilometre = bakimKaydi.Kilometre,
                SonrakiBakimKm = bakimKaydi.SonrakiBakimKm,
                BakimTuru = bakimKaydi.BakimTuru,
                AracPlaka = arac.Plaka
            };

            return new ServiceResult<BakimResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }
    }
}
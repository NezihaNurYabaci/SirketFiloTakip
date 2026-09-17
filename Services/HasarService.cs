using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Data;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Services
{
    public class HasarService
    {
        private readonly AppDbContext _context;

        public HasarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<HasarResponseDto>> HasarEkle(HasarCreateDto dto)
        {
            var arac = await _context.Araclar.FindAsync(dto.AracId);
            if (arac == null)
            {
                return new ServiceResult<HasarResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            Gorev? gorev = null;
            if (dto.GorevId != null)
            {
                gorev = await _context.Gorevler.FindAsync(dto.GorevId);

                if (gorev == null)
                {
                    return new ServiceResult<HasarResponseDto>
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Görev kaydı bulunamadı."
                    };
                }

                if (gorev.AracId != dto.AracId)
                {
                    return new ServiceResult<HasarResponseDto>
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Görev bu araca ait değil."
                    };
                }
            }

            var hasarKaydi = new HasarKaydi
            {
                AracId = dto.AracId,
                GorevId = dto.GorevId,
                Durum = dto.Durum,
                Tarih = dto.Tarih,
                Aciklama = dto.Aciklama,
                Maliyet = dto.Maliyet,
            };
            _context.HasarKayitlari.Add(hasarKaydi);
            await _context.SaveChangesAsync();

            var response = new HasarResponseDto
            {
                Id = hasarKaydi.Id,
                AracId = hasarKaydi.AracId,
                GorevId = hasarKaydi.GorevId,
                Durum = hasarKaydi.Durum,
                Maliyet = hasarKaydi.Maliyet,
                Tarih = hasarKaydi.Tarih,
                Aciklama = hasarKaydi.Aciklama,
                AracPlaka = arac.Plaka,
                GorevAmaci = gorev?.Amac
            };

            return new ServiceResult<HasarResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<List<HasarResponseDto>> GetHasarKayitlari()
        {
            var hasarKayitlari = await _context.HasarKayitlari.Include(h => h.Arac).Include(h => h.Gorev).ToListAsync();
            var response = hasarKayitlari.Select(h => new HasarResponseDto
            {
                Id = h.Id,
                AracId = h.AracId,
                GorevId = h.GorevId,
                Durum = h.Durum,
                Maliyet = h.Maliyet,
                Tarih = h.Tarih,
                Aciklama = h.Aciklama,
                AracPlaka = h.Arac.Plaka,
                GorevAmaci = h.Gorev?.Amac
            }).ToList();
            return response;
        }

        public async Task<HasarResponseDto?> GetHasarKaydi(int id)
        {
            var hasarKaydi = await _context.HasarKayitlari.Include(h => h.Arac).Include(h => h.Gorev).FirstOrDefaultAsync(y => y.Id == id);
            if (hasarKaydi == null)
            {
                return null;
            }

            var response = new HasarResponseDto
            {
                Id = hasarKaydi.Id,
                AracId = hasarKaydi.AracId,
                GorevId = hasarKaydi.GorevId,
                Durum = hasarKaydi.Durum,
                Maliyet = hasarKaydi.Maliyet,
                Tarih = hasarKaydi.Tarih,
                Aciklama = hasarKaydi.Aciklama,
                AracPlaka = hasarKaydi.Arac.Plaka,
                GorevAmaci = hasarKaydi.Gorev?.Amac
            };

            return response;
        }

        public async Task<bool> HasarKaydiSil(int id)
        {
            var hasarKaydiSil = await _context.HasarKayitlari.FindAsync(id);
            if (hasarKaydiSil == null) { return false; }
            _context.HasarKayitlari.Remove(hasarKaydiSil);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ServiceResult<HasarResponseDto>> HasarKaydiGuncelle(int id, HasarUpdateDto dto)
        {
            var hasarKaydi = await _context.HasarKayitlari.FindAsync(id);

            if (hasarKaydi == null)
            {
                return new ServiceResult<HasarResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Hasar kaydı bulunamadı."
                };
            }

            var arac = await _context.Araclar.FindAsync(dto.AracId);
            if (arac == null)
            {
                return new ServiceResult<HasarResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            Gorev? gorev = null;
            if (dto.GorevId != null)
            {
                gorev = await _context.Gorevler.FindAsync(dto.GorevId);

                if (gorev == null)
                {
                    return new ServiceResult<HasarResponseDto>
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Görev kaydı bulunamadı."
                    };
                }

                if (gorev.AracId != dto.AracId)
                {
                    return new ServiceResult<HasarResponseDto>
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Görev bu araca ait değil."
                    };
                }
            }

                hasarKaydi.AracId = dto.AracId;
                hasarKaydi.GorevId = dto.GorevId;
                hasarKaydi.Durum = dto.Durum;
                hasarKaydi.Maliyet = dto.Maliyet;
                hasarKaydi.Tarih = dto.Tarih;
                hasarKaydi.Aciklama = dto.Aciklama;
                await _context.SaveChangesAsync();

                var response = new HasarResponseDto
                {
                    Id = hasarKaydi.Id,
                    AracId = hasarKaydi.AracId,
                    GorevId = hasarKaydi.GorevId,
                    Durum = hasarKaydi.Durum,
                    Maliyet = hasarKaydi.Maliyet,
                    Tarih = hasarKaydi.Tarih,
                    Aciklama = hasarKaydi.Aciklama,
                    AracPlaka = arac.Plaka,
                    GorevAmaci = gorev?.Amac
                };

                return new ServiceResult<HasarResponseDto>
                {
                    Success = true,
                    Data = response,
                    ErrorMessage = null
                };
            

        }

    }
}

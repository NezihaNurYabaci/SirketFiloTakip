using Microsoft.EntityFrameworkCore;
using SirketFiloTakip.Data;
using SirketFiloTakip.DTOs;
using SirketFiloTakip.Models;

namespace SirketFiloTakip.Services
{
    public class GorevService
    {
        private readonly AppDbContext _context;

        public GorevService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<GorevResponseDto>> GorevEkle(GorevCreateDto dto)
        {
            var arac = await _context.Araclar.FindAsync(dto.AracId);

            if (arac == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunamadı."
                };
            }

            var calisan = await _context.Calisanlar.FindAsync(dto.CalisanId);

            if (calisan == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Çalışan kaydı bulunamadı."
                };
            }

            if (calisan.Ehliyet != arac.GerekliEhliyetSinifi)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Çalışanın ehliyet sınıfı bu araç için uygun değil."
                };
            }

            if (dto.PlanlananDonus <= dto.PlanlananCikis)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Planlanan dönüş tarihi çıkış tarihinden sonra olmalıdır."
                };
            }

            var aracCakisiyorMu = await _context.Gorevler.AnyAsync(g =>
                g.AracId == dto.AracId &&
                g.Durum != GorevDurumu.IptalEdildi &&
                g.Durum != GorevDurumu.Tamamlandi &&
                dto.PlanlananCikis < g.PlanlananDonus &&
                dto.PlanlananDonus > g.PlanlananCikis
            );

            if (aracCakisiyorMu)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç seçilen tarih aralığında başka bir görevde."
                };
            }

            var calisanCakisiyorMu = await _context.Gorevler.AnyAsync(g =>
                g.CalisanId == dto.CalisanId &&
                g.Durum != GorevDurumu.IptalEdildi &&
                g.Durum != GorevDurumu.Tamamlandi &&
                dto.PlanlananCikis < g.PlanlananDonus &&
                dto.PlanlananDonus > g.PlanlananCikis
            );

            if (calisanCakisiyorMu)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Çalışan seçilen tarih aralığında başka bir görevde."
                };
            }

            var gorev = new Gorev
            {
                Amac = dto.Amac,
                KalkisYeri = dto.KalkisYeri,
                VarisYeri = dto.VarisYeri,
                PlanlananCikis = dto.PlanlananCikis,
                PlanlananDonus = dto.PlanlananDonus,
                AracId = dto.AracId,
                CalisanId = dto.CalisanId,
                Durum = GorevDurumu.Planlandi
            };

            _context.Gorevler.Add(gorev);
            await _context.SaveChangesAsync();

            var response = new GorevResponseDto
            {
                Id = gorev.Id,
                Amac = gorev.Amac,
                Durum = gorev.Durum,
                KalkisYeri = gorev.KalkisYeri,
                VarisYeri = gorev.VarisYeri,
                PlanlananCikis = gorev.PlanlananCikis,
                PlanlananDonus = gorev.PlanlananDonus,
                GercekCikis = gorev.GercekCikis,
                GercekDonus = gorev.GercekDonus,
                BaslangicKm = gorev.BaslangicKm,
                BitisKm = gorev.BitisKm,
                AracId = gorev.AracId,
                AracPlaka = arac.Plaka,
                CalisanId = gorev.CalisanId,
                CalisanAdSoyad = calisan.AdSoyad
            };

            return new ServiceResult<GorevResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<List<GorevResponseDto>> GetGorevler()
        {
            var gorevler = await _context.Gorevler
                .Include(g => g.Arac)
                .Include(g => g.Calisan)
                .ToListAsync();

            var response = gorevler.Select(g => new GorevResponseDto
            {
                Id = g.Id,
                Amac = g.Amac,
                KalkisYeri = g.KalkisYeri,
                VarisYeri = g.VarisYeri,
                PlanlananCikis = g.PlanlananCikis,
                PlanlananDonus = g.PlanlananDonus,

                AracId = g.AracId,
                AracPlaka = g.Arac.Plaka,

                CalisanId = g.CalisanId,
                CalisanAdSoyad = g.Calisan.AdSoyad,

                Durum = g.Durum,
                GercekCikis = g.GercekCikis,
                GercekDonus = g.GercekDonus,
                BaslangicKm = g.BaslangicKm,
                BitisKm = g.BitisKm
            }).ToList();

            return response;
        }

        public async Task<GorevResponseDto?> GetGorev(int id)
        {
            var gorev = await _context.Gorevler
                .Include(g => g.Arac)
                .Include(g => g.Calisan)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gorev == null)
            {
                return null;
            }

            var response = new GorevResponseDto
            {
                Id = gorev.Id,
                Amac = gorev.Amac,
                KalkisYeri = gorev.KalkisYeri,
                VarisYeri = gorev.VarisYeri,
                PlanlananCikis = gorev.PlanlananCikis,
                PlanlananDonus = gorev.PlanlananDonus,

                AracId = gorev.AracId,
                AracPlaka = gorev.Arac.Plaka,

                CalisanId = gorev.CalisanId,
                CalisanAdSoyad = gorev.Calisan.AdSoyad,

                Durum = gorev.Durum,
                GercekCikis = gorev.GercekCikis,
                GercekDonus = gorev.GercekDonus,
                BaslangicKm = gorev.BaslangicKm,
                BitisKm = gorev.BitisKm
            };

            return response;
        }

        public async Task<ServiceResult<GorevResponseDto>> GorevGuncelle(
            int id,
            GorevUpdateDto dto)
        {
            var gorev = await _context.Gorevler.FindAsync(id);

            if (gorev == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Görev kaydı bulunmamaktadır."
                };
            }

            var arac = await _context.Araclar.FindAsync(dto.AracId);

            if (arac == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç kaydı bulunmamaktadır."
                };
            }

            var calisan = await _context.Calisanlar.FindAsync(dto.CalisanId);

            if (calisan == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Çalışan kaydı bulunmamaktadır."
                };
            }

            if (calisan.Ehliyet != arac.GerekliEhliyetSinifi)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Çalışanın ehliyet sınıfı bu araç için uygun değil."
                };
            }

            if (dto.PlanlananDonus <= dto.PlanlananCikis)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Planlanan dönüş tarihi çıkış tarihinden sonra olmalıdır."
                };
            }

            var aracCakisiyorMu = await _context.Gorevler.AnyAsync(g =>
                g.Id != id &&
                g.AracId == dto.AracId &&
                g.Durum != GorevDurumu.IptalEdildi &&
                g.Durum != GorevDurumu.Tamamlandi &&
                dto.PlanlananCikis < g.PlanlananDonus &&
                dto.PlanlananDonus > g.PlanlananCikis
            );

            if (aracCakisiyorMu)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Araç seçilen tarih aralığında başka bir görevde."
                };
            }

            var calisanCakisiyorMu = await _context.Gorevler.AnyAsync(g =>
                g.Id != id &&
                g.CalisanId == dto.CalisanId &&
                g.Durum != GorevDurumu.IptalEdildi &&
                g.Durum != GorevDurumu.Tamamlandi &&
                dto.PlanlananCikis < g.PlanlananDonus &&
                dto.PlanlananDonus > g.PlanlananCikis
            );

            if (calisanCakisiyorMu)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Çalışan seçilen tarih aralığında başka bir görevde."
                };
            }

            gorev.Amac = dto.Amac;
            gorev.KalkisYeri = dto.KalkisYeri;
            gorev.VarisYeri = dto.VarisYeri;
            gorev.PlanlananCikis = dto.PlanlananCikis;
            gorev.PlanlananDonus = dto.PlanlananDonus;
            gorev.AracId = dto.AracId;
            gorev.CalisanId = dto.CalisanId;

            await _context.SaveChangesAsync();

            var response = new GorevResponseDto
            {
                Id = gorev.Id,
                Amac = gorev.Amac,
                KalkisYeri = gorev.KalkisYeri,
                VarisYeri = gorev.VarisYeri,
                PlanlananCikis = gorev.PlanlananCikis,
                PlanlananDonus = gorev.PlanlananDonus,

                AracId = gorev.AracId,
                AracPlaka = arac.Plaka,

                CalisanId = gorev.CalisanId,
                CalisanAdSoyad = calisan.AdSoyad,

                Durum = gorev.Durum,
                GercekCikis = gorev.GercekCikis,
                GercekDonus = gorev.GercekDonus,
                BaslangicKm = gorev.BaslangicKm,
                BitisKm = gorev.BitisKm
            };

            return new ServiceResult<GorevResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<ServiceResult<GorevResponseDto>> GorevBaslat(int id)
        {
            var gorev = await _context.Gorevler
                .Include(g => g.Arac)
                .Include(g => g.Calisan)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gorev == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Görev kaydı bulunamadı."
                };
            }

            if (gorev.Durum != GorevDurumu.Planlandi)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Sadece planlanmış görevler başlatılabilir."
                };
            }

            gorev.Durum = GorevDurumu.DevamEdiyor;
            gorev.GercekCikis = DateTime.Now;
            gorev.BaslangicKm = gorev.Arac.Kilometre;
            gorev.Arac.Durum = AracDurumu.Gorevde;

            await _context.SaveChangesAsync();

            var response = new GorevResponseDto
            {
                Id = gorev.Id,
                Amac = gorev.Amac,
                KalkisYeri = gorev.KalkisYeri,
                VarisYeri = gorev.VarisYeri,
                PlanlananCikis = gorev.PlanlananCikis,
                PlanlananDonus = gorev.PlanlananDonus,

                AracId = gorev.AracId,
                AracPlaka = gorev.Arac.Plaka,

                CalisanId = gorev.CalisanId,
                CalisanAdSoyad = gorev.Calisan.AdSoyad,

                Durum = gorev.Durum,
                GercekCikis = gorev.GercekCikis,
                GercekDonus = gorev.GercekDonus,
                BaslangicKm = gorev.BaslangicKm,
                BitisKm = gorev.BitisKm
            };

            return new ServiceResult<GorevResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<ServiceResult<GorevResponseDto>> GorevTamamla(
            int id,
            GorevTamamlaDto dto)
        {
            var gorev = await _context.Gorevler
                .Include(g => g.Arac)
                .Include(g => g.Calisan)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gorev == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Görev kaydı bulunamadı."
                };
            }

            if (gorev.Durum != GorevDurumu.DevamEdiyor)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Sadece devam eden görevler tamamlanabilir."
                };
            }

            if (gorev.BaslangicKm == null)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Görevin başlangıç kilometresi bulunamadı."
                };
            }

            if (gorev.BaslangicKm > dto.BitisKm)
            {
                return new ServiceResult<GorevResponseDto>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Bitiş kilometresi başlangıç kilometresinden küçük olamaz."
                };
            }

            gorev.Durum = GorevDurumu.Tamamlandi;
            gorev.GercekDonus = DateTime.Now;
            gorev.BitisKm = dto.BitisKm;

            gorev.Arac.Kilometre = dto.BitisKm;
            gorev.Arac.Durum = AracDurumu.Musait;

            await _context.SaveChangesAsync();

            var response = new GorevResponseDto
            {
                Id = gorev.Id,
                Amac = gorev.Amac,
                KalkisYeri = gorev.KalkisYeri,
                VarisYeri = gorev.VarisYeri,
                PlanlananCikis = gorev.PlanlananCikis,
                PlanlananDonus = gorev.PlanlananDonus,

                AracId = gorev.AracId,
                AracPlaka = gorev.Arac.Plaka,

                CalisanId = gorev.CalisanId,
                CalisanAdSoyad = gorev.Calisan.AdSoyad,

                Durum = gorev.Durum,
                GercekCikis = gorev.GercekCikis,
                GercekDonus = gorev.GercekDonus,
                BaslangicKm = gorev.BaslangicKm,
                BitisKm = gorev.BitisKm
            };

            return new ServiceResult<GorevResponseDto>
            {
                Success = true,
                Data = response,
                ErrorMessage = null
            };
        }

        public async Task<bool> GorevSil(int id)
        {
            var gorev = await _context.Gorevler.FindAsync(id);

            if (gorev == null)
            {
                return false;
            }

            _context.Gorevler.Remove(gorev);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
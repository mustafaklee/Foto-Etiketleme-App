using WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Results;
using WebAPI.Models.Domain;
using WebAPI.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Logic
{

    public class PullDataFromDB
    {
        private readonly AppDbContext appDbContext;
        public PullDataFromDB(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }

        public async Task<IDataResult<FotoEtiketDto>> GetFoto(int doktorID,string baseUrl)
        {
            //ileride düzenleem için burda automapper kullanılmalıdır!
            try
            {
                var fotograflar = await appDbContext.FotografEtiket
                    .Where(fe => fe.DoktorId == doktorID && fe.EtiketId == null)
                    .Select(fe => new FotoDto
                    {
                        Id = fe.Fotograf.Id,
                        Path = $"{baseUrl}/{fe.Fotograf.FotografPath}"
                    })
                    .ToListAsync();

                var etiketler = await appDbContext.Etiket
                    .Select(f => new EtiketDto
                    {
                        Id = f.Id,
                        EtiketAd = f.EtiketAd
                    })
                    .ToListAsync();

                var fotoEtiketDto = new FotoEtiketDto{
                    Fotograflar= fotograflar,
                    Etiketler = etiketler
                };

                if(fotoEtiketDto.Fotograflar.Count == 0)
                {
                    return new SuccessDataResult<FotoEtiketDto>(
                    new FotoEtiketDto
                    {
                        Fotograflar = new List<FotoDto>(),
                        Etiketler = new List<EtiketDto>()
                    },
                    "Tüm fotoğraflar etiketlendi");
                }
                else
                {
                    return new SuccessDataResult<FotoEtiketDto>(fotoEtiketDto, "Fotoğraflar Başarıyla getirildi");
                }
            }

            catch(Exception ex)
            {
                return new ErrorDataResult<FotoEtiketDto>($"Bir hata meydana geldi{ex}");
            }
        }


        
        public async Task<IDataResult<FotoEtiketDto>> GetFotoByDate(
            int doktorID,
            string baseUrl,
            DateOnly startDate,
            DateOnly endDate)
        {
            // ileride düzenleme için burda automapper kullanılmalıdır!
            try
            {
                var labeledQuery = appDbContext.FotografEtiket
                    .Where(fe =>
                        fe.DoktorId == doktorID &&
                        fe.EtiketId != null &&
                        fe.EtiketTarihi.HasValue &&
                        fe.EtiketTarihi.Value >= startDate &&
                        fe.EtiketTarihi.Value <= endDate
                    );

                var fotograflar = await labeledQuery
                    .Select(fe => new FotoDto
                    {
                        Id = fe.Fotograf.Id,
                        Path = $"{baseUrl}/{fe.Fotograf.FotografPath}"
                    })
                    .ToListAsync();

                var etiketler = await appDbContext.Etiket
                    .Select(f => new EtiketDto
                    {
                        Id = f.Id,
                        EtiketAd = f.EtiketAd
                    })
                    .ToListAsync();

                var hasEtiket = await labeledQuery
                    .Select(fe => new EtiketDto
                    {
                        Id = fe.EtiketId!.Value,
                        EtiketAd = fe.Etiket!.EtiketAd
                    })
                    .ToListAsync();

                var resultDto = new FotoEtiketDto
                {
                    Fotograflar = fotograflar,
                    Etiketler = etiketler,
                    hasEtiket = hasEtiket
                };

                if (!fotograflar.Any())
                {
                    return new SuccessDataResult<FotoEtiketDto>(
                        resultDto,
                        $"Belirtilen {startDate:yyyy-MM-dd} – {endDate:yyyy-MM-dd} aralığında etiketli fotoğraf bulunamadı."
                    );
                }

                return new SuccessDataResult<FotoEtiketDto>(
                    resultDto,
                    $"Belirtilen {startDate:yyyy-MM-dd} – {endDate:yyyy-MM-dd} aralığında {fotograflar.Count} adet etiketli fotoğraf getirildi."
                );
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<FotoEtiketDto>($"Bir hata meydana geldi: {ex.Message}");
            }

        }




    }
}

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

        public async Task<IDataResult<List<FolderFotoDto>>> GetFoto(Guid doktorID, string baseUrl)
        {
            //ileride düzenleem için burda automapper kullanılmalıdır!
            try
            {
                var folders = await appDbContext.Folder
                    .Where(f => f.DoktorId == doktorID)
                    .Include(f => f.Fotograf)
                    .ToListAsync();

                var folderFotograflar = folders
                    .Select(folder => new FolderFotoDto
                    {
                        FolderId = folder.Id,
                        FolderPath = folder.FolderPath,
                        Fotograflar = folder.Fotograf.Select(f => new FotoDto
                        {
                            Id = f.Id,
                            Path = $"{baseUrl}/{folder.FolderPath}/{f.FotografPath}"
                        }).ToList()
                    }).ToList();

                return new SuccessDataResult<List<FolderFotoDto>>(folderFotograflar, "Klasörler ve fotoğraflar başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<FolderFotoDto>>($"Bir hata meydana geldi: {ex.Message}");
            }
        }

        public async Task<IDataResult<object>> GetBreastAndFinding()
        {
            try
            {
                var breastEtiketler = await appDbContext.BreastBirads
                    .Select(b => new
                    {
                        Id = b.Id,
                        EtiketAd = b.CategoryName
                    }).ToListAsync();

                var findingEtiketler = await appDbContext.FindingCategories
                    .Select(f => new
                    {
                        Id = f.Id,
                        EtiketAd = f.CategoryName
                    }).ToListAsync();

                var result = new
                {
                    BreastBirads = breastEtiketler,
                    FindingCategories = findingEtiketler
                };

                return new SuccessDataResult<object>(result, "Etiketler başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<object>($"Bir hata meydana geldi: {ex.Message}");
            }
        }



        //public async Task<IDataResult<FotoEtiketDto>> GetFotoByDate(
        //    Guid doktorID,
        //    string baseUrl,
        //    DateOnly startDate,
        //    DateOnly endDate)
        //{
        //    // ileride düzenleme için burda automapper kullanılmalıdır!
        //    try
        //    {
        //        var labeledQuery = appDbContext.FotografEtiket
        //            .Where(fe =>
        //                fe.DoktorId == doktorID &&
        //                fe.EtiketId != null &&
        //                fe.EtiketTarihi.HasValue &&
        //                fe.EtiketTarihi.Value >= startDate &&
        //                fe.EtiketTarihi.Value <= endDate
        //            );

        //        var fotograflar = await labeledQuery
        //            .Select(fe => new FotoDto
        //            {
        //                Id = fe.Fotograf.Id,
        //                Path = $"{baseUrl}/{fe.Fotograf.FotografPath}"
        //            })
        //            .ToListAsync();

        //        var etiketler = await appDbContext.Etiket
        //            .Select(f => new EtiketDto
        //            {
        //                Id = f.Id,
        //                EtiketAd = f.EtiketAd
        //            })
        //            .ToListAsync();

        //        var hasEtiket = await labeledQuery
        //            .Select(fe => new EtiketDto
        //            {
        //                Id = fe.EtiketId!.Value,
        //                EtiketAd = fe.Etiket!.EtiketAd
        //            })
        //            .ToListAsync();

        //        var resultDto = new FotoEtiketDto
        //        {
        //            Fotograflar = fotograflar,
        //            Etiketler = etiketler,
        //            hasEtiket = hasEtiket
        //        };

        //        if (!fotograflar.Any())
        //        {
        //            return new SuccessDataResult<FotoEtiketDto>(
        //                resultDto,
        //                $"Belirtilen {startDate:yyyy-MM-dd} – {endDate:yyyy-MM-dd} aralığında etiketli fotoğraf bulunamadı."
        //            );
        //        }

        //        return new SuccessDataResult<FotoEtiketDto>(
        //            resultDto,
        //            $"Belirtilen {startDate:yyyy-MM-dd} – {endDate:yyyy-MM-dd} aralığında {fotograflar.Count} adet etiketli fotoğraf getirildi."
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ErrorDataResult<FotoEtiketDto>($"Bir hata meydana geldi: {ex.Message}");
        //    }

        //}




    }
}

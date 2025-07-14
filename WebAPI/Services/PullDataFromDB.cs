using WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Results;
using WebAPI.Models.Dtos;
namespace WebAPI.Logic
{

    public class PullDataFromDB
    {
        private readonly AppDbContext appDbContext;
        public PullDataFromDB(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }



        public async Task<IDataResult<List<FolderFotoDto>>> GetLabeledFotos(int doktorId, string baseUrl)
        {
            try
            {
               
                var labeledPhotos = await appDbContext.Image
                    .Where(img =>
                        img.Folder.FolderDoctorEntities.Any(fd => fd.DoctorId == doktorId) &&
                       (img.BreastBiradsEntities.Any() || img.FindingCategoriesEntities.Any()))
                    .Select(img => new
                    {
                        img.Id,
                        img.FolderId,
                        img.Folder.FolderPath,
                        img.Folder.patient_age,
                        img.FotografPath,
                        img.laterality_id,
                        img.view_position_id,

                        Birads = img.BreastBiradsEntities
                                    .OrderBy(b => b.Id)
                                    .Select(b => (int?)b.BreastBiradsId)
                                    .FirstOrDefault(),

                        FindingCats = img.FindingCategoriesEntities
                                         .Select(fc => fc.FindingCategoriesId)
                    })
                    .AsNoTracking()
                    .ToListAsync();

                var result = labeledPhotos
                    .GroupBy(p => new { p.FolderId, p.FolderPath,p.patient_age })
                    .Select(g => new FolderFotoDto
                    {
                        FolderId = g.Key.FolderId,
                        FolderPath = g.Key.FolderPath,
                        PatientAge = g.Key.patient_age, 
                        Fotograflar = g.Select(p => new FotoDto
                        {
                            Id = p.Id,
                            Path = $"{baseUrl}/{p.FolderPath}/{p.FotografPath}",
                            laterality_id = p.laterality_id,
                            view_Position = p.view_position_id,
                            tags = new Tags
                            {
                                breast_birads = p.Birads ?? 0,
                                finding_categories = p.FindingCats.ToList()
                            }
                        })
                        .ToList()
                    })
                    .ToList();

                return new SuccessDataResult<List<FolderFotoDto>>(
                    result,
                    "Etiketlenmiş klasör fotoğrafları başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<FolderFotoDto>>(
                    $"Hata: {ex.Message}");
            }
        }



        public async Task<IDataResult<List<FolderFotoDto>>> GetFoto(int doktorId, string baseUrl, int page, int pageSize)
        {
            try
            {
                int skip = (page - 1) * pageSize;

                var etiketsizFotolar = await appDbContext.Image
                    .Where(img =>
                        img.Folder.FolderDoctorEntities.Any(fd => fd.DoctorId == doktorId) &&
                        !img.FindingCategoriesEntities.Any() &&
                        !img.BreastBiradsEntities.Any())
                    .OrderBy(img => img.FolderId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(img => new
                    {
                        FolderId = img.FolderId,
                        FolderPath = img.Folder.FolderPath,
                        PatientAge = img.Folder.patient_age,
                        Foto = new FotoDto
                        {
                            Id = img.Id,
                            Path = $"{baseUrl}/{img.Folder.FolderPath}/{img.FotografPath}",
                            laterality_id = img.laterality_id,
                            view_Position = img.view_position_id
                        }
                        })
                    .AsNoTracking()
                    .ToListAsync();

                var klasorler = etiketsizFotolar
                    .GroupBy(x => new { x.FolderId, x.FolderPath, x.PatientAge })
                    .Select(g => new FolderFotoDto
                    {
                        FolderId = g.Key.FolderId,
                        FolderPath = g.Key.FolderPath,
                        PatientAge = g.Key.PatientAge,
                        Fotograflar = g.Select(x => x.Foto).ToList()
                    })
                    .ToList();


                return new SuccessDataResult<List<FolderFotoDto>>(
                    klasorler,
                    "Fotoğraflar başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<FolderFotoDto>>(
                    $"Bir hata meydana geldi: {ex.Message}");
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

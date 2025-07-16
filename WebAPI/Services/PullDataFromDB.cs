using WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Results;
using WebAPI.Models.Dtos;
using WebAPI.Models.Domain;
namespace WebAPI.Logic
{

    public class PullDataFromDB
    {
        private readonly AppDbContext appDbContext;
        public PullDataFromDB(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }


        public async Task<IDataResult<List<FolderFotoDto>>> GetLabeledFolders(int doctorId)
        {
            try
            {
                var labeledFolderInfos = await appDbContext.Image
                    .Where(img => (img.BreastBiradsEntities.Any() || img.FindingCategoriesEntities.Any()) && (img.Folder.FolderDoctorEntities.Any(fd => fd.DoctorId == doctorId)))
                    .Select(img => new
                    {
                        img.FolderId,
                        img.Folder.FolderPath
                    })
                    .Distinct()
                    .AsNoTracking()
                    .ToListAsync();

                var result = labeledFolderInfos
                    .GroupBy(x => new { x.FolderId, x.FolderPath })
                    .Select(g => new FolderFotoDto
                    {
                        FolderId = g.Key.FolderId,
                        FolderPath = g.Key.FolderPath
                    })
                    .ToList();

                return new SuccessDataResult<List<FolderFotoDto>>(result, "Etiketlenmiş klasörler başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<FolderFotoDto>>($"Hata: {ex.Message}");
            }
        }



        public async Task<IDataResult<FolderFotoDto>> GetLabeledImages(int folderId, int doctorId, string baseUrl)
        {
            try
            {
                var labeledPhotos = await appDbContext.Image
                    .Where(img =>
                        img.FolderId == folderId &&
                        img.Folder.FolderDoctorEntities.Any(fd => fd.DoctorId == doctorId) && // Doktora aitlik kontrolü
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

                if (!labeledPhotos.Any())
                {
                    return new ErrorDataResult<FolderFotoDto>("Bu doktor ve folderId için etiketlenmiş fotoğraf bulunamadı.");
                }

                var first = labeledPhotos.First();

                var folderFoto = new FolderFotoDto
                {
                    FolderId = first.FolderId,
                    FolderPath = first.FolderPath,
                    PatientAge = first.patient_age,
                    Fotograflar = labeledPhotos.Select(p => new FotoDto
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
                    }).ToList()
                };

                return new SuccessDataResult<FolderFotoDto>(
                    folderFoto,
                    "Belirtilen klasör ve doktora ait etiketlenmiş fotoğraflar başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<FolderFotoDto>($"Hata: {ex.Message}");
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

    }
}

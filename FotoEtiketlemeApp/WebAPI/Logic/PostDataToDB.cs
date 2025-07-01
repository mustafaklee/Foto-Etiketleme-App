using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models.Domain;
using WebAPI.Models.Dtos;
using WebAPI.Repositories.Results;
namespace WebAPI.Logic
{
    public class PostDataToDB
    {
        private readonly AppDbContext appDbContext;

        public PostDataToDB(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;

        }
        public async Task<Repositories.Results.IResult> PostFoto(List<EtiketSecimDto> secimler, Guid doktorId)
        {
            try
            {
                foreach (var dto in secimler)
                {
                    // 1. FotografEtiket kaydı
                    var etiket = new FotografEtiket
                    {
                        FotografId = dto.ImageId,
                        BreastBiradsId = dto.BreastBirads
                    };
                    await appDbContext.FotografEtiket.AddAsync(etiket);

                    // 2. Her bir FindingCategoriesId için FindingCategoriesEntity oluştur
                    foreach (var catId in dto.FindingCategories)
                    {
                        var fce = new FindingCategoriesEntity
                        {
                            ImageId = dto.ImageId, // Bu, FotografId'ye karşılık gelir
                            FindingCategoriesId = catId
                        };
                        await appDbContext.FindingCategoriesEntities.AddAsync(fce);
                    }
                }

                await appDbContext.SaveChangesAsync();
                return new SuccessResult("Kayıt işlemi başarıyla gerçekleşti.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Veritabanına kaydederken bir hata meydana geldi: {ex.Message}");
            }
        }

    }
}

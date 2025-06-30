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
                    foreach (var catId in dto.FindingCategories)
                    {
                        var etiket = new FotografEtiket
                        {
                            imageId = dto.ImageId,
                            breast_biradsId = dto.BreastBirads,
                            finding_categoriesId = catId
                            
                        };
                        await appDbContext.FotografEtiket.AddAsync(etiket);
                    }
                }

                await appDbContext.SaveChangesAsync();
                return new SuccessResult("Kayıt İşlemi Başarıyla Gerçekleşti");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Veritabanına Kaydederken Bir hata meydana geldi {ex}");
            }
        }
    }
}

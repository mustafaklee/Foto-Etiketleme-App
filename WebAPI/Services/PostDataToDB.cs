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
        public async Task<Repositories.Results.IResult> PostFoto(List<EtiketSecimDto> secimler, int doctorId)
        {
            try
            {
                foreach (var secim in secimler)
                {
                    var image = await appDbContext.Image.FindAsync(secim.ImageId);
                    if (image == null)
                        continue;

                    
                    var oldBirads = appDbContext.BreastBiradsEntities
                        .Where(b => b.ImageId == secim.ImageId && b.DoctorId == doctorId);
                    appDbContext.BreastBiradsEntities.RemoveRange(oldBirads);

                    var oldFindings = appDbContext.FindingCategoriesEntities
                        .Where(fc => fc.ImageId == secim.ImageId && fc.DoctorId == doctorId);
                    appDbContext.FindingCategoriesEntities.RemoveRange(oldFindings);

                    
                    if (secim.BreastBirads > 0)
                    {
                        await appDbContext.BreastBiradsEntities.AddAsync(new BreastBiradsEntity
                        {
                            ImageId = secim.ImageId,
                            BreastBiradsId = secim.BreastBirads,
                            DoctorId = doctorId
                        });
                    }

                    foreach (var categoryId in secim.FindingCategories.Distinct())
                    {
                        await appDbContext.FindingCategoriesEntities.AddAsync(new FindingCategoriesEntity
                        {
                            ImageId = secim.ImageId,
                            FindingCategoriesId = categoryId,
                            DoctorId = doctorId
                        });
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

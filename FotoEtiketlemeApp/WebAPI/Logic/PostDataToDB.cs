using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
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
        public async Task<Repositories.Results.IResult> PostFoto(List<EtiketSecimDto> secimler , Guid doktorId)
        {
            try
            {
                foreach (var secim in secimler)
                {
                    var mevcut = await appDbContext.FotografEtiket
                        .FirstOrDefaultAsync(fe => fe.FotografId == secim.FotografId && doktorId == fe.DoktorId);

                    if (mevcut != null)
                    {
                        mevcut.EtiketId = secim.EtiketId;
                        mevcut.EtiketTarihi = DateOnly.FromDateTime(DateTime.Now);
                    }
                    else
                    {
                        return new ErrorResult($"FotografId {secim.FotografId} ve DoktorId {doktorId} ile eşleşen kayıt bulunamadı.");
                    }
                }

                await appDbContext.SaveChangesAsync();
                return new SuccessResult("Kayıt İşlemi Başarıyla Gerçekleşti");
            }
            catch(Exception ex)
            {
                return new ErrorResult($"Veritabanına Kaydederken Bir hata meydana geldi {ex}" );
            }
        }
    }
}

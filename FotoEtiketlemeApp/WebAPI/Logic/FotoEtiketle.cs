using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebAPI.Logic
{

    public class FotoEtiketle
    {
        private readonly AppDbContext appDbContext;
        public FotoEtiketle(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }

        public async Task<Object> GetFoto(int doktorID,string baseUrl)
        {

            var fotograflar = await appDbContext.FotografEtiket
            .Where(fe => fe.DoktorId == doktorID && fe.EtiketId == null)
            .Select(fe => new
            {
                Id = fe.Fotograf.Id,
                Path = $"{baseUrl}/{fe.Fotograf.FotografPath}"
            }).ToListAsync();


            var etiketler = await appDbContext.Etiket
                .Select(f => new { f.Id, f.EtiketAd })
                .ToListAsync();

            return new
            {
                Fotograflar = fotograflar,
                Etiketler = etiketler
            };
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebAPI.Data;
using WebAPI.Models.Domain;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotografEtiketle : ControllerBase
    {

        private readonly AppDbContext appDbContext;
        public FotografEtiketle(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }

        [HttpGet]
        public IActionResult GetFoto()
        {
            int doktorID = 1;

            var fotograflar = appDbContext.Fotograf
                .Where(f => appDbContext.FotografEtiket
                    .Where(fe => fe.FotografId == f.Id && fe.DoktorId == doktorID)
                    .Any(c => c.EtiketId != null)) // doktor henüz etiketlememiş
                .Select(f => new
                {
                    Id = f.Id,
                    FotografPath = f.FotografPath
                })
                .ToList();



            var etiketler = appDbContext.Etiket
                .Select(f=>new { f.Id ,f.EtiketAd })
                .ToList();


            if (fotograflar == null || !fotograflar.Any())
            {
                return NotFound("Etiketlenmiş fotoğraf bulunamadı.");
            }

            var paths = fotograflar
                .Select(f => $"{Request.Scheme}://{Request.Host}/{f.FotografPath}")
                .ToList();

            return Ok(new { Fotograflar = paths,
                            Etiketler = etiketler});
        }

    }
}
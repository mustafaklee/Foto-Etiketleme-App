using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebAPI.Data;
using WebAPI.Models.Domain;
using WebAPI.Models.Dtos;
using WebAPI.Logic;
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


        [HttpPost]
        [Route("PostFoto")]
        public IActionResult PostFoto([FromBody] List<EtiketSecimDto>  secimler)
        {
            int doktorId = 1;
            foreach (var secim in secimler)
            {
                var mevcut = appDbContext.FotografEtiket
                    .FirstOrDefault(fe => fe.FotografId == secim.FotografId && doktorId == fe.DoktorId);

                if (mevcut != null)
                {
                    mevcut.EtiketId = secim.EtiketId;
                    mevcut.EtiketTarihi = DateOnly.FromDateTime(DateTime.Now);
                }
                else
                {
                    return BadRequest($"FotografId {secim.FotografId} ve DoktorId {doktorId} ile eşleşen kayıt bulunamadı.");
                }
            }

            appDbContext.SaveChanges();
            return Ok();

        }



        [HttpGet]
        public async Task<IActionResult> GetFoto()
        {
            int doktorID = 1;
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var fotoEtiketle = new FotoEtiketle(appDbContext);
            var result = await fotoEtiketle.GetFoto(doktorID, baseUrl);

            return Ok(result);
        }

    }
}
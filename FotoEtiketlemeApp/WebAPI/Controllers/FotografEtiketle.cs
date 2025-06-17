using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Extensions;
using WebAPI.Models.Dtos;
using WebAPI.Logic;
using System.Security.Claims;
using WebAPI.Repositories.Results;
using Microsoft.AspNetCore.Authorization;
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FotografEtiketle : ControllerBase
    {

        private readonly AppDbContext appDbContext;
        public FotografEtiketle(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }


        [HttpPost]
        [Route("PostFoto")]
        public async Task<IActionResult> PostFoto([FromBody] List<EtiketSecimDto>  secimler)
        {
            Guid doktorId = User.GetUserId().Value;
            if (doktorId == null)
                return Unauthorized();

            var postDataToDb = new PostDataToDB(appDbContext);
            var result = await postDataToDb.PostFoto(secimler, doktorId);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetStats")]
        public async Task<IDataResult<Object>> GetStats()
        {
            try
            {
                Guid doktorId = User.GetUserId().Value;
                if (doktorId == null)
                    return new ErrorDataResult<object>($"Hata oluştu");

                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var etiketlenmis = await appDbContext.FotografEtiket
                    .Where(fe => fe.DoktorId == doktorId && fe.EtiketId != null)
                    .CountAsync();

                var bekleyen = await appDbContext.FotografEtiket
                    .Where(fe => fe.DoktorId == doktorId && fe.EtiketId == null)
                    .CountAsync();

                var stats = new
                {
                    Email = email,
                    Etiketlenmis = etiketlenmis,
                    Bekleyen = bekleyen
                };

                return new SuccessDataResult<object>(stats, "İstatistikler getirildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<object>($"Hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("GetStatsForAdmin")]
        public async Task<IDataResult<List<AdminIstatistikDto>>> GetStatsForAdmin()
        {
            try
            {
                var stats = await appDbContext.Doktor
                    .Select(d => new AdminIstatistikDto
                    {
                        Email = d.Email,
                        AtananFotoSayisi = d.FotografEtiketleri.Count(),
                        EtiketlenenFotoSayisi = d.FotografEtiketleri.Count(fe => fe.EtiketId != null),
                        BekleyenFotoSayisi = d.FotografEtiketleri.Count(fe => fe.EtiketId == null)
                    })
                    .ToListAsync();

                return new SuccessDataResult<List<AdminIstatistikDto>>(stats, "İstatistikler başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<AdminIstatistikDto>>($"Hata oluştu: {ex.Message}");
            }
        }



        [HttpGet("GetFoto")]
        public async Task<IActionResult> GetFoto([FromQuery] int count)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            Guid doktorId = User.GetUserId().Value;
            if (doktorId == null)
                return Unauthorized(); 

            var pullDataFromDb = new PullDataFromDB(appDbContext);

            var result = await pullDataFromDb.GetFoto(doktorId, baseUrl, count);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet]
        [Route("GetFotoByDate")]
        public async Task<IActionResult> GetFotoByDate(DateOnly startDate,DateOnly endDate)
        {
            Guid doktorId = User.GetUserId().Value;
            if (doktorId == null)
                return Unauthorized();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var pullDataFromDb = new PullDataFromDB(appDbContext);
            var result = await pullDataFromDb.GetFotoByDate(doktorId, baseUrl,startDate,endDate);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

    }
}
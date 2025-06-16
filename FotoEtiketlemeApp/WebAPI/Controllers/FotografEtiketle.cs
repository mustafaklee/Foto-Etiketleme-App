using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Extensions;
using WebAPI.Models.Dtos;
using WebAPI.Logic;
using System.Security.Claims;
using WebAPI.Repositories.Results;
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




        [HttpGet]
        [Route("GetFoto")]
        public async Task<IActionResult> GetFoto()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            Guid doktorId = User.GetUserId().Value;
            if (doktorId == null)
                return Unauthorized(); 

            var pullDataFromDb = new PullDataFromDB(appDbContext);

            var result = await pullDataFromDb.GetFoto(doktorId, baseUrl);

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
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models.Dtos;
using WebAPI.Logic;
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FotografEtiketle : ControllerBase
    {

        private readonly AppDbContext appDbContext;
        public FotografEtiketle(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }


        [HttpPost("PostFoto")]
        public async Task<IActionResult> PostFoto([FromBody] List<EtiketSecimDto> secimler)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            //string doktorId = User.GetUserId().Value;
            int doktorId = 1; // örnek, gerçek uygulamada JWT'den çekilecek
            //if (string.Empty(doktorId))
            //    return Unauthorized();
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

        [HttpGet("GetLabeledFotos")]
        public async Task<IActionResult> GetLabeledFotos()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            int doktorId = 2; // örnek, gerçek uygulamada JWT'den çekilecek
            //if (string.IsNullOrEmpty(doktorId))
            //    return Unauthorized();
            var pullDataFromDb = new PullDataFromDB(appDbContext);
            var result = await pullDataFromDb.GetLabeledFotos(doktorId, baseUrl);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }


        [HttpGet("GetFoto")]
        public async Task<IActionResult> GetFoto(int page = 1, int pageSize=40)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            int doktorId = 2; // örnek, gerçek uygulamada JWT'den çekilecek
            //if (string.IsNullOrEmpty(doktorId))
            //    return Unauthorized();
            var pullDataFromDb = new PullDataFromDB(appDbContext);
            var result = await pullDataFromDb.GetFoto(doktorId, baseUrl, page, pageSize);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpGet("GetBreastAndFinding")]
        public async Task<IActionResult> GetBreastAndFinding()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var pullDataFromDb = new PullDataFromDB(appDbContext);

            var result = await pullDataFromDb.GetBreastAndFinding();

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
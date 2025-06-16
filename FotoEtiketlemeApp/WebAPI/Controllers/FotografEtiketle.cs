using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebAPI.Data;
using WebAPI.Models.Domain;
using WebAPI.Models.Dtos;
using WebAPI.Logic;
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
            int doktorId = 1;
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
        [Route("GetFoto")]
        public async Task<IActionResult> GetFoto()
        {
            int doktorID = 1;
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var pullDataFromDb = new PullDataFromDB(appDbContext);
            var result = await pullDataFromDb.GetFoto(doktorID, baseUrl);

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
        public async Task<IActionResult> GetFotoByDate(DateOnly startDate,
            DateOnly endDate)
        {
            int doktorID = 1;
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var pullDataFromDb = new PullDataFromDB(appDbContext);
            var result = await pullDataFromDb.GetFotoByDate(doktorID, baseUrl,startDate,endDate);

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
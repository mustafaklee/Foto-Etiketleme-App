using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Logic;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public StatsController(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }

        [HttpGet("getStats")]
        public async Task<IActionResult> getStats(int doctor1Id, int doctor2Id)
        {
            //if (string.IsNullOrEmpty(doktorId))
            //    return Unauthorized();
            var statsControllerService = new StatsControllerService(appDbContext);
            var result = await statsControllerService.GetDifferenceLabeledImageCount(doctor1Id, doctor2Id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}

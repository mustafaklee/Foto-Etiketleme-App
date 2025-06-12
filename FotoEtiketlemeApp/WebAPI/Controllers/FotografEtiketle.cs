using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

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

        [HttpGet("{id}")]
        public IActionResult GetFoto(int id)
        {
            var foto = appDbContext.Fotograf.Find(id);

            if(foto == null)
            {
                return NotFound();
            }

            string path = $"{Request.Scheme}://{Request.Host}/" + foto.FotografPath;
            return Ok(new { url = path });
        }

    }
}

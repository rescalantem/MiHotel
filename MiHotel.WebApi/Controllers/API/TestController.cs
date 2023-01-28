using Microsoft.AspNetCore.Mvc;

namespace MiHotel.WebApi.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public ActionResult<bool> Get()
        {
            return Ok(true);
        }
    }
}

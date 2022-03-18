using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JsonResponder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RespondController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            string rawContent, randomChange;
            using (var contentStream = this.Request.Body)
            {
                using (var sr = new StreamReader(contentStream))
                {
                    rawContent = await sr.ReadToEndAsync();
                    Console.WriteLine(rawContent);
                }
            }
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(rawContent);
            if (json == null)
                return BadRequest();
            return Ok(json);
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketStore.Api.Model.Poster;

namespace TicketStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadPosterController : ControllerBase
    {
        private readonly ILogger<VerifyController> _log;
        private readonly IPosterUpdater _updater;

        public UploadPosterController(ILogger<VerifyController> log, IPosterUpdater updater)
        {
            _log = log;
            _updater = updater;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Poster poster)
        {
            try
            {
                _log.LogInformation($"Update event ID: {poster.eventId}, Image URI {poster.imageUrl}");
                var imageNameUrl = await _updater.Update(poster);
                return new OkObjectResult(imageNameUrl);
            } 
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return new BadRequestObjectResult("Failed to update poster");
            }
        }
    }
}

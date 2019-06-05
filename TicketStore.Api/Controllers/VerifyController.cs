using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TicketStore.Api.Data;
using TicketStore.Api.Model.Http;
using TicketStore.Api.Model.Validation;

namespace TicketStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private ApplicationContext _db;
        private const string _token = "Bearer pkR9vfZ9QdER53mf";

        public VerifyController(ApplicationContext context)
        {
            _db = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Barcode barcode)
        {
            if (HttpContext.Request.Headers["Authorization"] != _token)
            {
                return new UnauthorizedObjectResult(new UnauthorizedAnswer());
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new BadRequestAnswer());
            }

            var tickets = _db.Tickets.Where(t => t.Number == barcode.code).ToList();
            if (tickets.Count == 0)
            {
                return new BadRequestObjectResult(new InvalidCodeAnswer());
            }

            var ticket = tickets.First(t => t.Expired == false);
            if (ticket == null)
            {
                return new BadRequestObjectResult(new AlreadyVerifiedAnswer());
            }

            ticket.Expired = true;
            _db.Tickets.Update(ticket);
            _db.SaveChanges();
            return new OkObjectResult(new Answer("OK"));
        }
    }
}

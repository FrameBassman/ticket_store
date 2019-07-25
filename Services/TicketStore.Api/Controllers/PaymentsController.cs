using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DinkToPdf.Contracts;
using TicketStore.Api.Model;
using TicketStore.Api.Model.Email;
using TicketStore.Api.Model.Pdf;
using TicketStore.Data;
using TicketStore.Data.Model;

namespace TicketStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private ApplicationContext _db;
        private ILogger<PaymentsController> _log;
        private EmailService _emailService;
        private IConverter _converter;

        public PaymentsController(
            ApplicationContext context,
            ILogger<PaymentsController> log,
            IConverter pdfConverter,
            EmailService emailService
        )
        {
            _db = context;
            _log = log;
            _emailService = emailService;
            _converter = pdfConverter;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post(
            [FromForm] Boolean test_notification,
            [FromForm] String notification_type,
            [FromForm] String operation_id,
            [FromForm] Decimal amount,
            [FromForm] Decimal withdraw_amount,
            [FromForm] String currency,
            [FromForm] DateTime datetime,
            [FromForm] Boolean unaccepted,
            [FromForm] String email,
            [FromForm] String lastname,
            [FromForm] String sender,
            [FromForm] Boolean codepro,
            [FromForm] String label
        )
        {
            if (string.IsNullOrEmpty(email))
            {
                _log.LogInformation("Receive Yandex request without email");
                return new OkObjectResult("It's OK for yandex testing");
            }
            email = NormalizeEmail(email);
            
            // Hardcoded Cherdak account
            var receiver = "410019797958519";
            _log.LogInformation("Receive Yandex.Money request from {@0} about {@1}", email, label);
            if (!new Validator(
                    notification_type,
                    operation_id,
                    amount,
                    currency,
                    datetime,
                    sender,
                    codepro,
                    "",
                    label
                ).FromYandex()
            )
            {
                return new BadRequestObjectResult("Secret is not matching");
            }

//            var merchant = _db.Merchants.FirstOrDefault(m => m.YandexMoneyAccount == receiver);
//            if (merchant == null)
//            {
//                return new BadRequestObjectResult("Unknown merchant");
//            }
            
            var concert = _db.Events.FirstOrDefault(
                e => new LabelCalculator(e).Value() == label
            );
            if (concert == null)
            {
                return new BadRequestObjectResult("There is no event for merchant");
            }
            
            var tickets = CombineTickets(concert, new Payment { Email = email, Amount = withdraw_amount});
            
            if (tickets.Count == 0)
            {
                return new OkObjectResult("Payment is less than ticket cost");
            }
            var pdf = new Pdf(concert, tickets, _converter);
            _log.LogInformation("Combined PDF with barcodes");
            _emailService.SendTicket(email, pdf);
            return new OkObjectResult("OK");
        }

        private String NormalizeEmail(String origin)
        {
            return origin.Trim();
        }

        private List<Ticket> CombineTickets(Event concert, Payment payment)
        {
            _log.LogInformation("Receive payment: {@0}", payment);
            var ticketCost = concert.Roubles;
            var savedTickets = _db.Tickets.Where(t => t.Event == concert).ToList();
            var ticketsToSave = new List<Ticket>();
            int count = CalculateTicketsCount(payment.Amount, ticketCost);
            _log.LogInformation($"Combine {count} tickets");
            for (int i = 0; i < count; i++)
            {
                ticketsToSave.Add(new Ticket {
                    CreatedAt = DateTime.Now,
                    Number = new Algorithm(savedTickets.Concat(ticketsToSave).ToList()).Next(),
                    Roubles = ticketCost,
                    Expired = false,
                    Event = concert
                });
            }
            payment.Tickets = ticketsToSave;
            _db.Payments.Add(payment);
            _db.SaveChanges();
            return ticketsToSave;
        }

        private Int32 CalculateTicketsCount(Decimal amount, Decimal cost)
        {
            return Convert.ToInt32(
                Math.Truncate(
                    Decimal.Divide(amount, cost)
                )
            );
        }
    }
}

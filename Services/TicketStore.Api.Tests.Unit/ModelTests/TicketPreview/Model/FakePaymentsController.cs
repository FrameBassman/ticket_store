using System.Collections.Generic;
using System.Net.Http;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Logging;
using TicketStore.Api.Controllers;
using TicketStore.Api.Model.Email;
using TicketStore.Api.Model.PdfDocument.Model.BarcodeConverters;
using TicketStore.Api.Tests.Unit.Stubs;
using TicketStore.Data;
using TicketStore.Data.Model;

namespace TicketStore.Api.Tests.Unit.ModelTests.TicketPreview.Model
{
    public class FakePaymentsController : PaymentsController
    {
        public FakePaymentsController(
            ApplicationContext context,
            ILogger<PaymentsController> log,
            IConverter pdfConverter,
            Converter barcodeConverter,
            EmailService emailService,
            IHttpClientFactory clientFactory
        ) : base(context, log, pdfConverter, barcodeConverter, emailService, clientFactory)
        {
        }

        public override void SendTickets(Event concert, List<Ticket> tickets, string email)
        {
            EmailService.SendTicket(
                email,
                new DummyPdf(concert, tickets, PdfConverter, HttpClient)
            );
        }
    }
}
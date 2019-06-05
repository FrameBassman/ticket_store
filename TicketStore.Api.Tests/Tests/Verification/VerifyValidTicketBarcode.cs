using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using TicketStore.Api.Tests.Model;
using TicketStore.Api.Tests.Model.Services.Verify.Answers;
using TicketStore.Api.Tests.Tests.Fixtures;
using Xunit;

namespace TicketStore.Api.Tests.Tests.Verification
{
    public class VerifyValidTicketBarcode : AbstractFixtureTest
    {
        public VerifyValidTicketBarcode(ApiFixture fixture) : base (fixture) {}
        
        [Fact]
        public void SendExistBarcode_ReturnsOk()
        {
            // Arrange
            var email = "test4@test.test";
            Fixture.Api.SendPayment(email, 300.00m, 300.00m);
            var ticket = Fixture.Db.Tickets.First(t => t.Payment.Email == email);

            // Act
            var response = Fixture.Api.VerifyBarcode(ticket.Number);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(new OkAnswer().ToString(), response.Content);

            Fixture.Db.Entry(ticket).State = EntityState.Detached;
            Assert.True(Fixture.Db.Find<Ticket>(ticket.Id).Expired);
        }

        [Fact]
        public void SendNotExistBarcode_ReturnsNotFound()
        {
            // Act
            var response = Fixture.Api.VerifyBarcode("-1");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(new NotFoundAnswer().ToString(), response.Content);
        }
    }
}
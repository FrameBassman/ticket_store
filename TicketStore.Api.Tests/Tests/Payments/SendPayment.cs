using System.Linq;
using System.Net;
using Xunit;
using NHamcrest;
using TicketStore.Api.Tests.Tests.Fixtures;
using TicketStore.Api.Tests.Tests.Matchers;

namespace TicketStore.Api.Tests.Tests.Payments
{
    public class SendPayment : AbstractFixtureTest
    {
        public SendPayment(ApiFixture fixture) : base (fixture) {}

        [Fact]
        public void YandexSendPayment_ValidPayment_ReturnsOk()
        {
            // Arrange
            var before = Fixture.Db.Tickets.Count();
            
            // Act
            var response = Fixture.Api.SendPayment("framebassman@gmail.com", 299.00m, 300.00m);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            AssertWithTimeout.That(() => Fixture.Db.Tickets.Count(), Is.EqualTo(before));
        }

        [Fact]
        public void YandexSendPayment_InvalidPayment_ReturnsOk()
        {
            // Arrange
            var before = Fixture.Db.Tickets.Count();
            
            // Act
            var response = Fixture.Api.SendPayment("framebassman@gmail.com", 300.00m, 299.00m);
            var after = Fixture.Db.Tickets.Count();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            AssertWithTimeout.That(() => Fixture.Db.Tickets.Count(), Is.EqualTo(before + 1));
        }
    }
}

using System.Linq;
using System.Net;
using Xunit;
using NHamcrest;
using TicketStore.Api.Tests.Data;
using TicketStore.Api.Tests.Model;
using TicketStore.Api.Tests.Tests.Fixtures;
using TicketStore.Api.Tests.Tests.Matchers;

namespace TicketStore.Api.Tests.Tests.Payments
{
    public class SendPayment : AbstractFixtureTest
    {
        public SendPayment(ApiFixture fixture) : base (fixture) {}

        [Fact]
        public void YandexSendPayment_InvalidPayment_ReturnsOk()
        {
            // Arrange
            var sender = Merchant.YandexMoneyAccount;
            var label = new LabelCalculator(Events[0]).Value();
            var email = Generator.Email();
            var before = Fixture.Db.Tickets.Select(t => t.Payment.Email == email);

            // Act
            var response = Fixture.Api.SendPayment(
                sender,
                label,
                email,
                1.99m,
                2.00m
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            AssertWithTimeout.That(
                () => Fixture.Db.Tickets.Select(t => t.Payment.Email == email).Count(),
                Is.EqualTo(before.Count())
            );
            AssertWithTimeout.That(() => Fixture.FakeSender.EmailsForAddress(email).Data.Count, Is.EqualTo(0));
        }

        [Fact]
        public void YandexSendPayment_ValidPayment_ReturnsOk()
        {
            // Arrange
            var sender = Merchant.YandexMoneyAccount;
            var label = new LabelCalculator(Events[0]).Value();
            var email = Generator.Email();
            var before = Fixture.Db.Tickets.Where(t => t.Payment.Email == email).ToList();
            Fixture.Db.Tickets.RemoveRange(before);
            
            // Act
            var response = Fixture.Api.SendPayment(sender, label, email, 2.00m, 1.99m);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            AssertWithTimeout.That(
                () => Fixture.Db.Tickets.Count(t => t.Payment.Email == email),
                Is.EqualTo(before.Count() + 1)
            );
            AssertWithTimeout.That(() => Fixture.FakeSender.EmailsForAddress(email).Data.Count, Is.EqualTo(1));
        }
    }
}

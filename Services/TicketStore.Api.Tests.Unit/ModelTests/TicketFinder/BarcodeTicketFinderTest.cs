using System;
using Moq;
using TicketStore.Api.Model;
using TicketStore.Api.Model.Validation;
using TicketStore.Api.Tests.Unit.BaseTest;
using TicketStore.Api.Tests.Unit.Model;
using Xunit;

namespace TicketStore.Api.Tests.Unit.ModelTests
{
    public class BarcodeTicketFinderTest : DbBaseTest<ITicketFinder>
    {
        protected ITicketFinder Finder;
        protected DateTime _dbTime;
        public BarcodeTicketFinderTest() : base("barcode_ticket_finder") {
            // UTC should be stored in Database
            _dbTime = new DateTime(2019, 10, 7, 16, 00, 00, DateTimeKind.Utc);
            SeedTestData(_dbTime);
            SetupFinder(_dbTime);
        }

        [Fact]
        public void BarcodeVerificationMethod_TicketExist()
        {
            var turnstileScan = new BarcodeTurnstileScan("11111");

            var ticket = Finder.Find(turnstileScan);

            Assert.Equal("1111122222", ticket.Number);
        }

        [Fact]
        public void BarcodeVerificationMethod_ConcertNotFound()
        {
            var turnstileScan = new BarcodeTurnstileScan("55555");

            var ex = Assert.Throws<Exception>(() => Finder.Find(turnstileScan));

            Assert.Equal("Method: Barcode. Concert is not found for ticket", ex.Message);
        }

        [Fact]
        public void BarcodeVerificationMethod_TicketNotFound()
        {
            var turnstileScan = new BarcodeTurnstileScan("123");

            var ex = Assert.Throws<Exception>(() => Finder.Find(turnstileScan));

            Assert.Equal("Method: Barcode. Ticket not found in Database", ex.Message);
        }

        [Fact]
        public void BarcodeVerificationMethod_TooLateForConcert()
        {
            var now = _dbTime.AddHours(15);
            SetupFinder(now);
            var turnstileScan = new BarcodeTurnstileScan("11111");

            var ex = Assert.Throws<Exception>(() => Finder.Find(turnstileScan));

            Assert.Equal("Method: Barcode. Too late for concert, it's happend 15 hours ago", ex.Message);
        }

        [Fact]
        public void BarcodeVerificationMethod__TooEarlyForConcert()
        {
            var now = _dbTime.AddHours(-15);
            SetupFinder(now);
            var turnstileScan = new BarcodeTurnstileScan("11111");;

            var ex = Assert.Throws<Exception>(() => Finder.Find(turnstileScan));

            Assert.Equal("Method: Barcode. Too early for concert, it will happen in 15 hours", ex.Message);
        }

        protected void SetupFinder(DateTime date)
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(mock => mock.Now).Returns(date);

            Finder = new BarcodeTicketFinder(Db, dateTimeProviderMock.Object);
        }
    }
}

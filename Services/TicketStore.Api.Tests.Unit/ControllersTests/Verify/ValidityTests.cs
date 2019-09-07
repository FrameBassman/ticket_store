using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketStore.Api.Model.Validation;
using Xunit;

namespace TicketStore.Api.Tests.Unit.ControllersTests.Verify
{
    public class ValidityTests : VerifyControllerBaseTest
    {
        public ValidityTests() : base("validity")
        {
            // UTC should be stored in Database
            var dbTime = new DateTime(2019, 10, 4, 16, 00, 00, DateTimeKind.Utc);
            SeedTestData(dbTime);
        }

        [Fact]
        public void TicketIsNotFoundInDatabase_ReturnsBadRequest()
        {
            // Arrange
            var turnstileScan = new TurnstileScan {
                code = "123",
                method = "Manual"
            };
            
            // Act
            var result = Controller.Post(turnstileScan);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var json = JsonConvert.SerializeObject((result as BadRequestObjectResult).Value);
            Assert.Equal("{\"message\":\"cannot find code in database\"}", json);
        }

        [Fact]
        public void TicketDoesntMatchAnyConcert_ReturnsBadRequest()
        {
            // Arrange
            var turnstileScan = new TurnstileScan {
                code = "5555566666",
                method = "Manual"
            };
            
            // Act
            var result = Controller.Post(turnstileScan);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var json = JsonConvert.SerializeObject((result as BadRequestObjectResult).Value);
            Assert.Equal("{\"message\":\"no concert found for this ticket\"}", json);
        }

        [Fact]
        public void TicketHasExpired_ReturnsOk()
        {
            // Arrange
            var turnstileScan = new TurnstileScan {
                code = "3333344444",
                method = "Manual"
            };
            
            // Act
            var result = Controller.Post(turnstileScan);
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var json = JsonConvert.SerializeObject((result as OkObjectResult).Value);
            Assert.Equal("{\"message\":\"OK\",\"concertLabel\":\"Test artist — 4 октября 2019\",\"used\":true}", json);
        }

        [Fact]
        public void TicketIsValid_ReturnsOk()
        {
            // Arrange
            var turnstileScan = new TurnstileScan {
                code = "1111122222",
                method = "Manual"
            };
            
            // Act
            var result = Controller.Post(turnstileScan);
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var json = JsonConvert.SerializeObject((result as OkObjectResult).Value);
            Assert.Equal("{\"message\":\"OK\",\"concertLabel\":\"Test artist — 4 октября 2019\",\"used\":false}", json);
        }
    }
}

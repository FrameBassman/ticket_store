using AspNetCore.Yandex.ObjectStorage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TicketStore.Api.Controllers;
using TicketStore.Api.Model;
using TicketStore.Api.Model.Poster;

namespace TicketStore.Api.Tests.Unit.ControllersTests.UploadPoster
{
    public abstract class UploadPosterControllerBaseTest : ControllersBaseTest<VerifyController>
    {
        protected readonly UploadsController Controller;
        protected UploadPosterControllerBaseTest(string databaseName) : base(databaseName)
        {
            var updater = GetUpdater();
            Controller = new UploadsController(Logger, updater);
        }

        private PosterUpdater GetUpdater()
        {
            var log = new Mock<ILogger<PosterUpdater>>();
            var dbUpdaterLog = new Mock<ILogger<PosterDbUpdater>>();
            var yandexStorageOptions = new YandexStorageOptions();
            var options = Options.Create<YandexStorageOptions>(yandexStorageOptions);
            var storage = new Mock<YandexStorageService>(options);
            var reader = new Mock<IPosterReader>();
            var dbUpdater = new PosterDbUpdater(Db, dbUpdaterLog.Object);
            var guidProvider = new Mock<IGuidProvider>();
            guidProvider.Setup(mock => mock.NewGuid()).Returns("g-u-i-d");
            return new PosterUpdater(log.Object, storage.Object, reader.Object, dbUpdater, guidProvider.Object);
        }
    }
}

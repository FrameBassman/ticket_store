using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TicketStore.Api.Model.Email
{
    public class EmailStrategy
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _conf;
        private readonly ILogger _log;
        
        public EmailStrategy(IHostingEnvironment env, IConfiguration conf, ILogger log)
        {
            _env = env;
            _conf = conf;
            _log = log;
        }

        public EmailService Service()
        {
            if (_env.IsEnvironment("Test"))
            {
                return new FakeSenderService(_env, _conf, _log);
            }
            else
            {
                return new YandexService(_env, _conf, _log);
            }
        }
    }
}
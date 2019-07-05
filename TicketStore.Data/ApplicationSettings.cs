using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace TicketStore.Data
{
    public class ApplicationSettings
    {
        private readonly String _environmentName;
        private readonly Boolean _isInsideDocker; 
        
        public ApplicationSettings()
        {
            _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT",
                                   EnvironmentVariableTarget.Process)
                               ?? "Development";
            _isInsideDocker =
                Convert.ToBoolean(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER",
                    EnvironmentVariableTarget.Process));
        }

        public String ConnectionString()
        {
            var candidate = BuildConfiguration().GetConnectionString("DefaultConnection");
            if (_environmentName == "Test")
            {
                if (_isInsideDocker)
                {
                    return candidate.Replace("$HOST","postgres");
                }
                else
                {
                    var dockerHostEnv = Environment.GetEnvironmentVariable("DOCKER_HOST", EnvironmentVariableTarget.Machine);
                    if (string.IsNullOrEmpty(dockerHostEnv))
                    {
                        return candidate.Replace("$HOST", "localhost");
                    }
                    else
                    {
                        var dockerHost = new UriBuilder(dockerHostEnv).Host;
                        return candidate.Replace("$HOST", dockerHost);
                    }
                }
            }
            return candidate;
        }
        
        private IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(CalculateBasePath())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private String CalculateBasePath()
        {
            var dirname = "./DataConfigs";
            var info = new DirectoryInfo(dirname);
            if (!info.Exists)
            {
                Console.WriteLine("There is no directory via address {0}", info.FullName);
                throw new FileNotFoundException("There is no file via address {0}", info.FullName);
            }
            Console.WriteLine("Base path for configs: {0}", info.FullName);
            return info.FullName;
        }
    }
}
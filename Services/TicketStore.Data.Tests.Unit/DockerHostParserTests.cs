using System;
using TicketStore.Data.Parsers;
using Xunit;

namespace TicketStore.Data.Tests.Unit
{
    public class DockerHostParserTests
    {
        [Fact]
        public void DockerHostIsEmpty_ShouldChangeToLocalhost()
        {
            Environment.SetEnvironmentVariable("DOCKER_HOST", "", EnvironmentVariableTarget.Process);
            var parser = new DockerHostParser("Host=$DOCKER_HOST$;Port=5432");
            Assert.Equal("Host=localhost;Port=5432", parser.Parse());
        }

        [Fact]
        public void DockerHostWasSetup_ShouldChangeToValue()
        {
            Environment.SetEnvironmentVariable("DOCKER_HOST", "tcp://192.168.0.1", EnvironmentVariableTarget.Process);
            var parser = new DockerHostParser("Host=$DOCKER_HOST$;Port=5432");
            Assert.Equal("Host=192.168.0.1;Port=5432", parser.Parse());
        }
    }
}
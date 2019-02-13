using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace WebApi.Tests
{
    public class UnitTest1
    {
        TestServer server1, server2;

        TestServer Config1Host
        {
            get
            {
                if (server1 == null)
                {
                    var hostBuilder = WebHost.CreateDefaultBuilder()
                        .ConfigureAppConfiguration(configBuilder =>
                        {
                            configBuilder.AddJsonFile("appsettings.config1.json");
                        })
                        .UseStartup<Startup>();

                    server1 = new TestServer(hostBuilder);
                }

                return server1;
            }
        }

        TestServer Config2Host
        {
            get
            {
                if (server2 == null)
                {
                    var hostBuilder = WebHost.CreateDefaultBuilder()
                        .ConfigureAppConfiguration(configBuilder =>
                        {
                            configBuilder.AddJsonFile("appsettings.config2.json");
                        })
                        .UseStartup<Startup>();

                    server2 = new TestServer(hostBuilder);
                }

                return server2;
            }
        }

        [Fact]
        public async Task TestWithMultipleServers()
        {
            // Setup clients from the two different servers
            var client1 = Config1Host.CreateClient();
            var client2 = Config2Host.CreateClient();

            // Server 1
            Assert.Equal("Hello from config1!", await client1.GetStringAsync("api/values"));

            // Server 2

            Assert.Equal("Hello from config2!", await client2.GetStringAsync("api/values"));

            // Confirm CallCount was shared
            Assert.Equal("2", await client1.GetStringAsync("api/values/callcount"));
            Assert.Equal("2", await client2.GetStringAsync("api/values/callcount"));
        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParksVictoriaApi.Tests
{
    [TestClass]
    public class FileServiceIntegrationTests : WebApplicationFactory<Startup>
    {
        private static TestContext _testContext;
        private static WebApplicationFactory<Startup> _factory;
        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // will be called after the `ConfigureServices` from the Startup
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                    .AddJsonFile("integrationsettings.json")
                    .Build();

                config.AddConfiguration(Configuration);
            });
        }

        [ClassInitialize]
        public static void FileServiceIntegrationTestsInit(TestContext testContext)
        {
            _testContext = testContext;
            _factory = new WebApplicationFactory<Startup>();
        }

        [TestMethod]
        public async Task Test_ArtistGetAsyncNotFound()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/File?fileName=appsettings2.json");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        [TestMethod]
        public async Task Test_InvalidFileDirectory()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/File?fileName=appsettings2.json");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        [TestMethod]
        public async Task Test_IncorrectFileName()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/File?fileName=appsettings2.json");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        [TestMethod]
        public async Task Test_ReturnFileResponse()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/File?fileName=appsettings.json");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}

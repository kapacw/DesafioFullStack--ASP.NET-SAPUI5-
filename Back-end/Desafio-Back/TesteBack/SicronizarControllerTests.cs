using Desafio_Back;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace TesteBack.Tests
{
    public class SincronizarControllerTests
    {
        private readonly HttpClient _client;

        public SincronizarControllerTests()
        {
            var app = TestStartup.CreateApp("Db_Sincronizacao");
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task SincronizarBanco_DeveRetornarOkEPreencherBanco()
        {
            var response = await _client.PostAsync("/sync", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<SincronizarResponse>();
            Assert.NotNull(content);
            Assert.True(content.totalUsuarios > 0);
            Assert.True(content.totalTarefas > 0);
        }

        public class SincronizarResponse
        {
            public string mensagem { get; set; }
            public int totalUsuarios { get; set; }
            public int totalTarefas { get; set; }
        }
    }
}

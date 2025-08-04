using Desafio_Back;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace TesteBack.Tests
{
    public class TarefasControllerTests
    {
        private HttpClient CreateClient(string dbName)
        {
            var app = TestStartup.CreateApp(dbName);
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            return factory.CreateClient();
        }

        [Fact]
        public async Task GetTarefas_DeveRetornarComPaginacaoEFiltro()
        {
            var client = CreateClient("Db_GetTarefas");
            var response = await client.GetAsync("/todos?page=1&pageSize=2&title=null&sort=title&order=asc");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);

            Assert.NotNull(result);
            Assert.NotNull(result.items);
        }

        [Fact]
        public async Task GetTarefaPorId_DeveRetornarTarefa()
        {
            var client = CreateClient("Db_GetTarefa");
            var response = await client.GetAsync("/todos/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            dynamic tarefa = JsonConvert.DeserializeObject(content);

            Assert.Equal(1, (int)tarefa.id);
            Assert.NotNull(tarefa.title);
        }

        [Fact]
        public async Task PutTarefa_DeveAtualizarStatus()
        {
            var client = CreateClient("Db_PutStatus");
            var dto = new { completed = true };
            var response = await client.PutAsJsonAsync("/todos/1", dto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            dynamic tarefa = JsonConvert.DeserializeObject(content);

            Assert.True((bool)tarefa.completed);
        }

        [Fact]
        public async Task PutTarefa_DeveRespeitarLimiteDeIncompletas()
        {
            var client = CreateClient("Db_PutLimiteIncompletas");
            var dto = new { completed = false };
            var response = await client.PutAsJsonAsync("/todos/15", dto);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Contains("5 tarefas incompletas", content);
            }
        }
    }
}
using Desafio_Back;
using Desafio_Back.Data;
using Desafio_Back.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TesteBack.Tests
{
    public class TestStartup
    {
        public static WebApplication CreateApp(string dbName)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddDbContext<TarefaContext>(options =>
                options.UseInMemoryDatabase(dbName));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TarefaContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Usuarios.AddRange(new[]
                {
                    new Usuario { Id = 1 },
                    new Usuario { Id = 2 },
                    new Usuario { Id = 3 }
                });

                db.Tarefas.AddRange(new[]
                {
                    new Tarefa { Id = 1, Title = "Estudar", Completed = false, UserId = 1 },
                    new Tarefa { Id = 2, Title = "Ler livro", Completed = false, UserId = 1 },
                    new Tarefa { Id = 3, Title = "Escrever", Completed = true, UserId = 1 },
                    new Tarefa { Id = 4, Title = "Testar código", Completed = true, UserId = 2 },
                    new Tarefa { Id = 5, Title = "Revisar", Completed = false, UserId = 2 },
                    new Tarefa { Id = 6, Title = "Limite 1", Completed = false, UserId = 3 },
                    new Tarefa { Id = 7, Title = "Limite 2", Completed = false, UserId = 3 },
                    new Tarefa { Id = 8, Title = "Limite 3", Completed = false, UserId = 3 },
                    new Tarefa { Id = 9, Title = "Limite 4", Completed = false, UserId = 3 },
                    new Tarefa { Id = 10, Title = "Limite 5", Completed = false, UserId = 3 },
                    new Tarefa { Id = 15, Title = "Bloqueio de tarefa", Completed = true, UserId = 3 }
                });

                db.SaveChanges();
            }

            app.MapControllers();
            return app;
        }
    }
}
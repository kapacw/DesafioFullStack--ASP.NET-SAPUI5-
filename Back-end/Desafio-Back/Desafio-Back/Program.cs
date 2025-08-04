using Desafio_Back.Data;
using Microsoft.EntityFrameworkCore;

var BaseUrl = "AQUI VAI SEU LOCALHOST, lembra de colocar em string";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<TarefaContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase1"))
    );

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(BaseUrl)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

namespace Desafio_Back
{
    public partial class Program { }
}

using AutoMapper;
using Desafio_Back.Data;
using Desafio_Back.Data.Dtos.TarefaDtos;
using Desafio_Back.Data.Dtos.UsuarioDtos;
using Desafio_Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Desafio_Back.Controllers;

[ApiController]
[Route("sync")]
public class SicronizarController : ControllerBase
{
    private readonly TarefaContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;

    public SicronizarController(TarefaContext context, IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _db = context;
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
    }

    // POST: Sincroniza os dados do banco de dados com da API fornecida
    [HttpPost]
    public async Task<IActionResult> SincronizarBanco()
    {
        var client = _httpClientFactory.CreateClient();
        var tarefasAPI = await client.GetFromJsonAsync<List<SyncTarefaDto>>("https://jsonplaceholder.typicode.com/todos");

        if (tarefasAPI == null || !tarefasAPI.Any())
        {
            return BadRequest(new { mensagem = "Nenhuma tarefa encontrada na API." });
        }

        // Transação única para IDENTITY_INSERT
        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            // Limpa tabelas
            _db.Tarefas.RemoveRange(_db.Tarefas);
            _db.Usuarios.RemoveRange(_db.Usuarios);
            await _db.SaveChangesAsync();

            // 🔹 Mapeia e insere USUÁRIOS com Id manual
            var usuariosDto = tarefasAPI
                .Where(t => t.UserId > 0 && t.Id > 0)
                .Select(t => t.UserId)
                .Distinct()
                .Select(id => new CreateUsuarioDto { Id = id })
                .ToList();

            var usuarios = _mapper.Map<List<Usuario>>(usuariosDto);

            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Usuarios ON");
            _db.Usuarios.AddRange(usuarios);
            await _db.SaveChangesAsync();
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Usuarios OFF");

            // Mapeia e insere TAREFAS com Id manual
            var tarefas = _mapper.Map<List<Tarefa>>(tarefasAPI);

            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Tarefas ON");
            _db.Tarefas.AddRange(tarefas);
            await _db.SaveChangesAsync();
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Tarefas OFF");

            await transaction.CommitAsync();

            return Ok(new
            {
                mensagem = "Sincronização concluída com sucesso.",
                totalUsuarios = usuarios.Count,
                totalTarefas = tarefas.Count
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new
            {
                mensagem = "Erro ao sincronizar banco de dados.",
                erro = ex.Message,
                inner = ex.InnerException?.Message
            });
        }
    }


}

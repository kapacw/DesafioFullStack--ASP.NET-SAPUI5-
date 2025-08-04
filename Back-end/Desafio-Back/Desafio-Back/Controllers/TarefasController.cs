using AutoMapper;
using Desafio_Back.Data;
using Desafio_Back.Data.Dtos.TarefaDtos;
using Desafio_Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Back.Controllers;

[ApiController]
[Route("todos")]
public class TarefasController : ControllerBase
{
    private readonly TarefaContext _db;
    private readonly IMapper _mapper;

    public TarefasController(TarefaContext context, IMapper mapper)
    {
        _db = context;
        _mapper = mapper;
    }

    // GET: Busca a lista 'Tarefas' na qual pode ou não conter filtro através de parâmetros:  página, quantidade de itens da página, título, ordenado por título ou id
    // e se está em ordem crescente ou decrescente
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadTarefaDto>>> ObterTodasTarefas(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? title = null,
            [FromQuery] string sort = "title",
            [FromQuery] string order = "asc"
        )
    {
        if (page <= 0)
            return BadRequest(new { mensagem = "A página deve ser maior ou diferente que 0!" });

        if (pageSize <= 0)
            return BadRequest(new { mensagem = "O tamanho da página deve ser maior ou diferente que 0!" });

        var sortLower = sort.ToLower();
        if (sortLower != "title" && sortLower != "id")
            return BadRequest(new { mensagem = "Para definir um tipo de ordenação deve declarar 'title' se deseja ordenar por título ou 'id' por id de cada item." });

        var orderLower = order.ToLower();
        if (orderLower != "asc" && orderLower != "desc")
            return BadRequest(new { mensagem = "Para definir ordem da lista deve declarar 'asc' para ordem ascendente ou 'desc' para ordem descendente." });


        IQueryable<Tarefa> query = _db.Tarefas;

        // Filtro por título
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(t => t.Title.Contains(title));
        }

        // Ordenação dinâmica 
        query = (sort.ToLower(), order.ToLower()) switch
        {
            ("title", "asc") => query.OrderBy(t => t.Title),
            ("title", "desc") => query.OrderByDescending(t => t.Title),
            ("id", "asc") => query.OrderBy(t => t.Id),
            ("id", "desc") => query.OrderByDescending(t => t.Id),
            _ => query.OrderBy(t => t.Title) // default
        };

        // Paginação
        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!items.Any())
        {
            return NotFound(new { mensagem = "Nenhuma tarefa encontrada." });
        }

        var tarefasDto = _mapper.Map<List<ReadTarefaDto>>(items);

        return Ok(new
        {
            page,
            pageSize,
            totalItems,
            items = tarefasDto
        });
    }

    // GET: Busca 'Tarefa' por id
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadTarefaDto>> ObterTarefaPorID(int id)
    {
        if (id <= 0) return BadRequest(new { mensagem = "Id inválido." });

        var tarefa = await _db.Tarefas.FirstOrDefaultAsync(t => t.Id == id);

        if (tarefa == null) return NotFound(new { mensagem = "Tarefa não encontrada." });

        var tarefaDto = _mapper.Map<ReadTarefaDto>(tarefa);

        return Ok(tarefaDto);
    }

    // PUT: Busca a 'Tarefa' por id e verifica se pode ou não alterar a propiedade 'completed' para falso caso for a intenção.
    [HttpPut("{id}")]
    public async Task<ActionResult<ReadTarefaDto>> EditarStatusTarefa(int id, [FromBody] UpdateTarefaDto dto)
    {
        if (id <= 0) return BadRequest(new { mensagem = "Id inválido." });

        var tarefa = await _db.Tarefas.FirstOrDefaultAsync(t => t.Id == id);

        if (tarefa == null) return NotFound(new { mensagem = "Id não encontrado." });

        // Aplicar regra de negócio enquanto marcar tarefa como incompleta.
        if (dto.Completed == false && tarefa.Completed == true)
        {
            var tarefasIncompletas = await _db.Tarefas
                .Where(t => t.UserId == tarefa.UserId && !t.Completed)
                .CountAsync();

            if (tarefasIncompletas >= 5)
            {
                return BadRequest(new
                {
                    mensagem = $"O usuário '{tarefa.UserId}' já possui 5 tarefas incompletas. Não é permitido marcar outra como incompleta."
                });
            }
        }

        // Atualiza os dados com AutoMapper.
        _mapper.Map(dto, tarefa);
        await _db.SaveChangesAsync();

        var tarefaAtualizada = _mapper.Map<ReadTarefaDto>(tarefa);
        return Ok(tarefaAtualizada);
    }
}

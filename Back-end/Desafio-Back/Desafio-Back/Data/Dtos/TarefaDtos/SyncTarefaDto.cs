using Desafio_Back.Interfaces;

namespace Desafio_Back.Data.Dtos.TarefaDtos;

public class SyncTarefaDto : ISyncAPI
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
}

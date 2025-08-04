namespace Desafio_Back.Data.Dtos.TarefaDtos;

public class ReadTarefaDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
    public int UserId { get; set; }
}

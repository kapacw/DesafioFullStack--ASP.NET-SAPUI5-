using System.ComponentModel.DataAnnotations;

namespace Desafio_Back.Data.Dtos.TarefaDtos;

public class UpdateTarefaDto
{
    [Required(ErrorMessage = "Boolean obrigatório")]
    public bool Completed { get; set; }
}

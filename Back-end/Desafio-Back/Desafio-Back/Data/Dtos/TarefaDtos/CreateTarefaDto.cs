using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_Back.Data.Dtos.TarefaDtos;

public class CreateTarefaDto
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "Título obrigatório")]
    [StringLength(255, ErrorMessage = "O tamanho do título não pode exceder 255 caracteres")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Boolean obrigatório")]
    public bool Completed { get; set; }
    [Required]
    public int UserId { get; set; }
}

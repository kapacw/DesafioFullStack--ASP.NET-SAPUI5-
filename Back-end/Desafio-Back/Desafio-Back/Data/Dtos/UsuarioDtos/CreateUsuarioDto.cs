using System.ComponentModel.DataAnnotations;

namespace Desafio_Back.Data.Dtos.UsuarioDtos;

public class CreateUsuarioDto
{
    [Required]
    public int Id { get; set; }
}

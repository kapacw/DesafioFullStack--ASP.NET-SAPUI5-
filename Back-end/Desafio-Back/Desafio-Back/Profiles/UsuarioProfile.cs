using AutoMapper;
using Desafio_Back.Data.Dtos.UsuarioDtos;
using Desafio_Back.Models;

namespace Desafio_Back.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDto, Usuario>();
    }
}

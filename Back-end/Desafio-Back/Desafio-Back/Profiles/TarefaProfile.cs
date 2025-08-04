using AutoMapper;
using Desafio_Back.Data.Dtos.TarefaDtos;
using Desafio_Back.Models;

namespace Desafio_Back.Profiles;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<CreateTarefaDto, Tarefa>();
        CreateMap<Tarefa, ReadTarefaDto>();
        CreateMap<UpdateTarefaDto, Tarefa>();

        CreateMap<SyncTarefaDto, Tarefa>();
    }
}

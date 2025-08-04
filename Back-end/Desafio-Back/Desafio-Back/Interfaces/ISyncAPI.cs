namespace Desafio_Back.Interfaces;

public interface ISyncAPI
{
    int UserId { get; set; }
    int Id { get; set; }
    string Title { get; set; }
    bool Completed { get; set; }
}

using Desafio_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Back.Data;

public class TarefaContext : DbContext
{
    public TarefaContext(DbContextOptions<TarefaContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Tarefa> Tarefas {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>()
            .HasOne(t => t.Usuario)
            .WithMany(u => u.Tarefas)
            .HasForeignKey(t => t.UserId);
    }
}

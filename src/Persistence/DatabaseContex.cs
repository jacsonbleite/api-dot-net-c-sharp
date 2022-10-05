using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Persistence;

public class DatabaseContex : DbContext
{
    public DatabaseContex(DbContextOptions<DatabaseContex> options) : base(options)
    {

    }

    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Contrato> Contratos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Pessoa>(tabela => {
            tabela.HasKey(e => e.Id);
            tabela
                .HasMany(e => e.contratos)
                .WithOne()
                .HasForeignKey(c => c.PessoaId);
        });

        builder.Entity<Contrato>(tabela => {
            tabela.HasKey( e => e.Id);
        });
    }
}
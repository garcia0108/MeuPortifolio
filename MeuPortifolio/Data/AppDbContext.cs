using MeuPortfolio.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MeuPortfolio.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Perfil> Perfis => Set<Perfil>();
    public DbSet<Habilidade> Habilidades => Set<Habilidade>();
    public DbSet<RedeSocial> RedesSociais => Set<RedeSocial>();
    public DbSet<Projeto> Projetos => Set<Projeto>();
    public DbSet<Midia> Midias => Set<Midia>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Projeto>().HasIndex(p => p.Slug).IsUnique();
        mb.Entity<Projeto>().Ignore(p => p.Tecnologias);
        mb.Entity<Projeto>().HasMany(p => p.Midias).WithOne(m => m.Projeto).OnDelete(DeleteBehavior.Cascade);
    }
}

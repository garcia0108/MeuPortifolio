using Microsoft.EntityFrameworkCore;
using MeuPortfolio.Data;
using MeuPortfolio.Models;

namespace MeuPortfolio.Services;

public interface IPortfolioService
{
    Task<Perfil> ObterPerfilAsync();
    Task<List<Projeto>> ObterProjetosAsync();
    Task<Projeto?> ObterProjetoPorSlugAsync(string slug);
}

public class PortfolioService : IPortfolioService
{
    private readonly AppDbContext _db;
    public PortfolioService(AppDbContext db) => _db = db;

    public async Task<Perfil> ObterPerfilAsync() =>
        await _db.Perfis.Include(p => p.Habilidades).Include(p => p.RedesSociais).FirstAsync();

    public Task<List<Projeto>> ObterProjetosAsync() =>
        _db.Projetos.OrderBy(p => p.Ordem).ToListAsync();

    public Task<Projeto?> ObterProjetoPorSlugAsync(string slug) =>
        _db.Projetos.Include(p => p.Midias).FirstOrDefaultAsync(p => p.Slug == slug);
}


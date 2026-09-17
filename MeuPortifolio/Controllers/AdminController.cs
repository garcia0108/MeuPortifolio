using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeuPortfolio.Data;
using MeuPortfolio.Models;

namespace MeuPortfolio.Controllers;

[Route("admin")]
[Authorize]
public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _cfg;
    private readonly IWebHostEnvironment _env;

    public AdminController(AppDbContext db, IConfiguration cfg, IWebHostEnvironment env)
    { _db = db; _cfg = cfg; _env = env; }

    // ---------- LOGIN ----------
    [AllowAnonymous, HttpGet("login")]
    public IActionResult Login() => View();

    [AllowAnonymous, HttpPost("login")]
    public async Task<IActionResult> Login(string usuario, string senha)
    {
        if (usuario == _cfg["Admin:Usuario"] && senha == _cfg["Admin:Senha"])
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario) },
                CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Erro = "Usuário ou senha inválidos.";
        return View();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    // ---------- PROJETOS ----------
    [HttpGet("")]
    public async Task<IActionResult> Index() =>
        View(await _db.Projetos.Include(p => p.Midias).OrderBy(p => p.Ordem).ToListAsync());

    [HttpGet("projeto/{id?}")]
    public async Task<IActionResult> Editar(int? id)
    {
        var projeto = id is null ? new Projeto() : await _db.Projetos.Include(p => p.Midias).FirstOrDefaultAsync(p => p.Id == id);
        return projeto is null ? NotFound() : View(projeto);
    }

    [HttpPost("projeto/{id?}")]
    public async Task<IActionResult> Editar(int? id, Projeto form, IFormFile? capa)
    {
        var projeto = id is null ? new Projeto() : await _db.Projetos.FindAsync(id);
        if (projeto is null) return NotFound();

        projeto.Titulo = form.Titulo;
        projeto.Slug = form.Slug.Trim().ToLower().Replace(' ', '-');
        projeto.ResumoCurto = form.ResumoCurto;
        projeto.DescricaoCompleta = form.DescricaoCompleta;
        projeto.TecnologiasTexto = form.TecnologiasTexto;
        projeto.RepositorioUrl = form.RepositorioUrl;
        projeto.DemoUrl = form.DemoUrl;
        projeto.Ordem = form.Ordem;
        if (capa is not null) projeto.CapaUrl = await SalvarArquivoAsync(capa, "img/projetos");

        if (id is null) _db.Projetos.Add(projeto);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Editar), new { id = projeto.Id });
    }

    [HttpPost("projeto/{id}/excluir")]
    public async Task<IActionResult> Excluir(int id)
    {
        var projeto = await _db.Projetos.FindAsync(id);
        if (projeto is not null) { _db.Projetos.Remove(projeto); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    // ---------- MÍDIAS ----------
    [HttpPost("projeto/{id}/midia")]
    public async Task<IActionResult> AdicionarMidia(int id, TipoMidia tipo, string? legenda, string? urlYoutube, IFormFile? arquivo)
    {
        var midia = new Midia { ProjetoId = id, Tipo = tipo, Legenda = legenda ?? "" };

        if (tipo == TipoMidia.YouTube && !string.IsNullOrWhiteSpace(urlYoutube))
            midia.Url = ConverterParaEmbed(urlYoutube);
        else if (arquivo is not null)
            midia.Url = await SalvarArquivoAsync(arquivo, tipo == TipoMidia.Video ? "videos" : "img/projetos");
        else
            return RedirectToAction(nameof(Editar), new { id });

        _db.Midias.Add(midia);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Editar), new { id });
    }

    [HttpPost("midia/{id}/excluir")]
    public async Task<IActionResult> ExcluirMidia(int id)
    {
        var midia = await _db.Midias.FindAsync(id);
        if (midia is null) return NotFound();
        _db.Midias.Remove(midia);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Editar), new { id = midia.ProjetoId });
    }

    // ---------- PERFIL ----------
    [HttpGet("perfil")]
    public async Task<IActionResult> Perfil() =>
        View(await _db.Perfis.Include(p => p.Habilidades).Include(p => p.RedesSociais).FirstAsync());

    [HttpPost("perfil")]
    public async Task<IActionResult> Perfil(Perfil form, IFormFile? foto)
    {
        var perfil = await _db.Perfis.FirstAsync();
        perfil.Nome = form.Nome; perfil.Cargo = form.Cargo; perfil.Resumo = form.Resumo;
        perfil.Email = form.Email; perfil.Telefone = form.Telefone; perfil.Localizacao = form.Localizacao;
        perfil.CurriculoUrl = form.CurriculoUrl;
        if (foto is not null) perfil.FotoUrl = await SalvarArquivoAsync(foto, "img");
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Perfil));
    }

    [HttpPost("habilidade")]
    public async Task<IActionResult> AdicionarHabilidade(string nome, int nivel, string icone)
    {
        var perfil = await _db.Perfis.FirstAsync();
        _db.Habilidades.Add(new Habilidade { PerfilId = perfil.Id, Nome = nome, Nivel = nivel, Icone = icone });
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Perfil));
    }

    [HttpPost]
    public async Task<IActionResult> AtualizarIconeHabilidade(int id, string icone)
    {
        var h = await _db.Habilidades.FindAsync(id);
        if (h != null)
        {
            h.Icone = icone?.Trim();
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Perfil");
    }


    [HttpPost("habilidade/{id}/excluir")]
    public async Task<IActionResult> ExcluirHabilidade(int id)
    {
        var h = await _db.Habilidades.FindAsync(id);
        if (h is not null) { _db.Habilidades.Remove(h); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Perfil));
    }

    [HttpPost("rede")]
    public async Task<IActionResult> AdicionarRede(string nome, string url, string icone)
    {
        var perfil = await _db.Perfis.FirstAsync();
        _db.RedesSociais.Add(new RedeSocial { PerfilId = perfil.Id, Nome = nome, Url = url, Icone = icone });
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Perfil));
    }

    [HttpPost("rede/{id}/excluir")]
    public async Task<IActionResult> ExcluirRede(int id)
    {
        var r = await _db.RedesSociais.FindAsync(id);
        if (r is not null) { _db.RedesSociais.Remove(r); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Perfil));
    }

    // ---------- HELPERS ----------
    private async Task<string> SalvarArquivoAsync(IFormFile arquivo, string pasta)
    {
        var dir = Path.Combine(_env.WebRootPath, pasta);
        Directory.CreateDirectory(dir);
        var nome = $"{Guid.NewGuid():N}{Path.GetExtension(arquivo.FileName)}";
        await using var fs = new FileStream(Path.Combine(dir, nome), FileMode.Create);
        await arquivo.CopyToAsync(fs);
        return $"/{pasta}/{nome}";
    }

    private static string ConverterParaEmbed(string url)
    {
        // aceita youtube.com/watch?v=ID, youtu.be/ID ou já embed
        if (url.Contains("/embed/")) return url;
        var id = url.Contains("v=") ? url.Split("v=")[1].Split('&')[0] : url.Split('/').Last().Split('?')[0];
        return $"https://www.youtube.com/embed/{id}";
    }
}

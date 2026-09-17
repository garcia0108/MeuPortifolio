using Microsoft.AspNetCore.Mvc;
using MeuPortfolio.Services;

namespace MeuPortfolio.Controllers;

public class HomeController : Controller
{
    private readonly IPortfolioService _service;
    public HomeController(IPortfolioService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        ViewBag.Projetos = await _service.ObterProjetosAsync();
        return View(await _service.ObterPerfilAsync());
    }

    public async Task<IActionResult> Projeto(string slug)
    {
        var projeto = await _service.ObterProjetoPorSlugAsync(slug);
        return projeto is null ? NotFound() : View(projeto);
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MeuPortfolio.Data;
using MeuPortfolio.Models;
using MeuPortfolio.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' não encontrada. Verifique appsettings.json ou User Secrets.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => { o.LoginPath = "/admin/login"; o.ExpireTimeSpan = TimeSpan.FromHours(8); });
builder.Services.AddAuthorization();

var app = builder.Build();

// Cria o banco e insere o perfil inicial se não existir
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    if (!db.Perfis.Any())
    {
        db.Perfis.Add(new Perfil
        {
            Nome = "Seu Nome",
            Cargo = "Desenvolvedor .NET",
            Resumo = "Edite este texto no painel /admin.",
            FotoUrl = "/img/perfil.jpg",
            Email = "seuemail@exemplo.com",
            Habilidades = new() { new() { Nome = "C#", Nivel = 90, Icone = "devicon-csharp-plain" } },
            RedesSociais = new() { new() { Nome = "GitHub", Url = "https://github.com/seuusuario", Icone = "bi bi-github" } }
        });
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();   // precisa vir antes
app.UseAuthorization();

app.MapControllerRoute(
    name: "projeto",
    pattern: "projetos/{slug}",
    defaults: new { controller = "Home", action = "Projeto" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

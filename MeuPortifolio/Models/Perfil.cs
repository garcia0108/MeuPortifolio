namespace MeuPortfolio.Models;

public class Perfil
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Cargo { get; set; } = "";
    public string Resumo { get; set; } = "";
    public string FotoUrl { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string Localizacao { get; set; } = "";
    public string CurriculoUrl { get; set; } = "";
    public List<Habilidade> Habilidades { get; set; } = new();
    public List<RedeSocial> RedesSociais { get; set; } = new();
}



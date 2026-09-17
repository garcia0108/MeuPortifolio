namespace MeuPortfolio.Models
{
    public class RedeSocial
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Url { get; set; } = "";
        public string? Icone { get; set; }

        public int PerfilId { get; set; }
        public Perfil? Perfil { get; set; }
    }
}

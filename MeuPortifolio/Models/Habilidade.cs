namespace MeuPortfolio.Models
{
    public class Habilidade
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public int Nivel { get; set; }
        public string? Icone { get; set; }

        public int PerfilId { get; set; }
        public Perfil? Perfil { get; set; }
    }
}

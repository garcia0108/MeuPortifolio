using System.ComponentModel.DataAnnotations;

namespace MeuPortfolio.Models;

public class Projeto
{
    public int Id { get; set; }
    [Required] public string Slug { get; set; } = "";
    [Required] public string Titulo { get; set; } = "";
    public string ResumoCurto { get; set; } = "";
    public string DescricaoCompleta { get; set; } = "";
    public string CapaUrl { get; set; } = "";
    public string TecnologiasTexto { get; set; } = "";   // separado por vírgula
    public string? RepositorioUrl { get; set; }
    public string? DemoUrl { get; set; }
    public int Ordem { get; set; }
    public List<Midia> Midias { get; set; } = new();

    public IEnumerable<string> Tecnologias =>
        TecnologiasTexto.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}

public enum TipoMidia { Imagem, Video, YouTube }

public class Midia
{
    public int Id { get; set; }
    public int ProjetoId { get; set; }
    public Projeto? Projeto { get; set; }
    public TipoMidia Tipo { get; set; }
    public string Url { get; set; } = "";
    public string Legenda { get; set; } = "";
}

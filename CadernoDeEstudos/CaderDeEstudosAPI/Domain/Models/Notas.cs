namespace CaderDeEstudosAPI.Domain.Models {
    public class Notas {
        public int NotasId { get; set; }
        public string Titulo { get; set; }
        public string? Conteudo { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public Caderno Caderno { get; set; } = new Caderno();
    }
}

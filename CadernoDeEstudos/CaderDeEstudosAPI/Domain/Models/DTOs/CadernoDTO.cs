namespace CaderDeEstudosAPI.Domain.Models.DTOs {
    public class CadernoDTO {
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}

namespace CaderDeEstudosAPI.Domain.Models.DTOs {
    public class NotasDTO {
        public string Titulo { get; set; }
        public string? Conteudo { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public CadernoDTO CadernoDTO { get; set; } = new CadernoDTO();
    }
}

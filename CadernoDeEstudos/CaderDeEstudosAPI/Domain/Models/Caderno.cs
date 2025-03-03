namespace CaderDeEstudosAPI.Domain.Models {
    public class Caderno {
        public int CadernoId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        //public List<Notas> Notas { get; set; } = new List<Notas>();
    }
}

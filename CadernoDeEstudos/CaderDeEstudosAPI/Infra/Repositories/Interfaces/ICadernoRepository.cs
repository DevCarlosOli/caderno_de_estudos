using CaderDeEstudosAPI.Domain.Models;

namespace CaderDeEstudosAPI.Infra.Repositories.Interfaces {
    public interface ICadernoRepository {
        Task<List<Caderno>> FindAllCadernosAsync();
        Task<Caderno> AddCadernoAsync(Caderno caderno);
    }
}

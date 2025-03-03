using CaderDeEstudosAPI.Domain.Models;

namespace CaderDeEstudosAPI.Infra.Repositories.Interfaces {
    public interface INotasRepository {
        Task<List<Notas>> FindAllNotasAsync();
        Task<Notas> FindNotasByIdAsync(int notasId);
        Task<Notas> AddNotasAsync(Notas notas);
        Task<Notas> UpdateNotasAsync(int notasId, Notas notas);
        Task<int> DeleteNotasAsync(int notasId);
    }
}

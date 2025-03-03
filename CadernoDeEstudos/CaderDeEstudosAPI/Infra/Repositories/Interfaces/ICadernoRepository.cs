using CaderDeEstudosAPI.Domain.Models;

namespace CaderDeEstudosAPI.Infra.Repositories.Interfaces {
    public interface ICadernoRepository {
        Task<List<Caderno>> FindAllCadernosAsync();
        Task<Caderno> FindCadernoByIdAsync(int cadernoId);
        Task<Caderno> AddCadernoAsync(Caderno caderno);
        Task<Caderno> UpdateCadernoAsync(int cadernoId, Caderno caderno);
        Task<int> DeleteCadernoAsync(int cadernoId);
    }
}

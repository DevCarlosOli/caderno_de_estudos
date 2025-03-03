using CaderDeEstudosAPI.Domain.Models;

namespace CaderDeEstudosAPI.Application.Services.Interfaces {
    public interface ICadernoService {
        Task<List<Caderno>> FindAllCadernosAsync();
        Task<Caderno> FindCadernoByIdAsync(int id);
        Task<Caderno> AddCadernoAsync(Caderno caderno);
        Task<Caderno> UpdateCadernoAsync(int cadernoId, Caderno caderno);
        Task<int> DeleteCadernoAsync(int cadernoId);
    }
}

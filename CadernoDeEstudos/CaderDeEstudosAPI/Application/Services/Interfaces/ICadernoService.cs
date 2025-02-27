using CaderDeEstudosAPI.Domain.Models;

namespace CaderDeEstudosAPI.Application.Services.Interfaces {
    public interface ICadernoService {
        Task<List<Caderno>> FindAllCadernosAsync();
        Task<Caderno> AddCadernoAsync(Caderno caderno);
    }
}

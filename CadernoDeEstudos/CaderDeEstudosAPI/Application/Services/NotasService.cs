using CaderDeEstudosAPI.Application.Services.Interfaces;
using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Infra.Repositories.Interfaces;

namespace CaderDeEstudosAPI.Application.Services {
    public class NotasService : INotasService {
        private readonly INotasRepository _notasRepository;

        public NotasService(INotasRepository notasRepository) {
            _notasRepository = notasRepository;
        }
        public async Task<List<Notas>> FindAllNotasAsync() => await _notasRepository.FindAllNotasAsync();

        public async Task<Notas> FindNotasByIdAsync(int notasId) => await _notasRepository.FindNotasByIdAsync(notasId);

        public async Task<Notas> AddNotasAsync(Notas notas) => await _notasRepository.AddNotasAsync(notas);

        public async Task<Notas> UpdateNotasAsync(int notasId, Notas notas) => await _notasRepository.UpdateNotasAsync(notasId, notas);

        public async Task<int> DeleteNotasAsync(int notasId) => await _notasRepository.DeleteNotasAsync(notasId);
    }
}

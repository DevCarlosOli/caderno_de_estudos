using CaderDeEstudosAPI.Application.Services.Interfaces;
using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Domain.Models.DTOs;
using CaderDeEstudosAPI.Infra.Repositories;
using CaderDeEstudosAPI.Infra.Repositories.Interfaces;

namespace CaderDeEstudosAPI.Application.Services {
    public class CadernoService : ICadernoService {
        private readonly ICadernoRepository _cadernoRepository;

        public CadernoService(ICadernoRepository cadernoRepository) {
            _cadernoRepository = cadernoRepository;
        }

        public async Task<List<Caderno>> FindAllCadernosAsync() {
            var cadernos = await _cadernoRepository.FindAllCadernosAsync();

            if (cadernos == null || !cadernos.Any()) {
                return null;
            }
            return cadernos;
        }

        public async Task<Caderno> FindCadernoByIdAsync(int id) => await _cadernoRepository.FindCadernoByIdAsync(id);

        public async Task<Caderno> AddCadernoAsync(Caderno caderno) => await _cadernoRepository.AddCadernoAsync(caderno);

        public async Task<Caderno> UpdateCadernoAsync(int cadernoId, Caderno caderno) => await _cadernoRepository.UpdateCadernoAsync(cadernoId, caderno);

        public async Task<int> DeleteCadernoAsync(int cadernoId) => await _cadernoRepository.DeleteCadernoAsync(cadernoId);
    }
}

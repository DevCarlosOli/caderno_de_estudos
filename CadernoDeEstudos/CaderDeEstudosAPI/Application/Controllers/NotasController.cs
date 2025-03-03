using AutoMapper;
using CaderDeEstudosAPI.Application.Services.Interfaces;
using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Domain.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaderDeEstudosAPI.Application.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class NotasController : ControllerBase {
        private readonly INotasService _notasService;
        private readonly IMapper _mapper;

        public NotasController(INotasService notasService, IMapper mapper) {
            _notasService = notasService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notas>>> GetAllNotasAsync() {
            try {
                var notas = await _notasService.FindAllNotasAsync();
                return Ok(notas);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{notasId}")]
        public async Task<ActionResult<Notas>> GetNotasByIdAsync(int notasId) {
            try {
                var notas = await _notasService.FindNotasByIdAsync(notasId);
                return Ok(notas);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Notas>> PostNotasAsync([FromBody]NotasDTO notasDTO) {
            try {
                if (string.IsNullOrEmpty(notasDTO.Titulo) || string.IsNullOrEmpty(notasDTO.Conteudo))
                    return BadRequest("Título e conteúdo não podem ficar vazios!");

                var notas = _mapper.Map<Notas>(notasDTO);
                var result = await _notasService.AddNotasAsync(notas);
                return Ok(result);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Notas>> PutNotasAsync([FromQuery]int notasId, [FromBody]NotasDTO notasDto) {
            try {
                var notas = _mapper.Map<Notas>(notasDto);
                var result = await _notasService.UpdateNotasAsync(notasId, notas);
                return Ok(result);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteNotasAsync(int notasId) {
            var notas = await _notasService.DeleteNotasAsync(notasId);
            return Ok(notas);
        }
    }
}

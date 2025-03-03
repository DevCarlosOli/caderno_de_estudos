using AutoMapper;
using CaderDeEstudosAPI.Application.Services.Interfaces;
using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Domain.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaderDeEstudosAPI.Application.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CadernoController : ControllerBase {
        private readonly ICadernoService _cadernoService;
        private readonly IMapper _mapper;

        public CadernoController(ICadernoService cadernoService, IMapper mapper) {
            _cadernoService = cadernoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Caderno>>> GetAllCadernosAsync() {
            try {
                var cadernos = await _cadernoService.FindAllCadernosAsync();
                return Ok(cadernos);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }            
        }

        [HttpGet("{cadernoId}")]
        public async Task<ActionResult<Caderno>> GetCadernoById(int cadernoId) {
            try {
                var caderno = await _cadernoService.FindCadernoByIdAsync(cadernoId);
                return Ok(caderno);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Caderno>> PostCadernoAsync([FromBody]CadernoDTO cadernoDto) {
            try {
                if (string.IsNullOrEmpty(cadernoDto.Nome))
                    return BadRequest("O Nome do caderno não pode ficar vazio!");

                var caderno = _mapper.Map<Caderno>(cadernoDto);

                var result = await _cadernoService.AddCadernoAsync(caderno);
                return Ok(result);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Caderno>> PutCadernoAsync([FromQuery] int cadernoId, [FromBody]CadernoDTO cadernoDTO) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var caderno = _mapper.Map<Caderno>(cadernoDTO);

                var result = await _cadernoService.UpdateCadernoAsync(cadernoId, caderno);
                return Ok(result);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteCadernoAsync([FromQuery]int cadernoID) {
            try {
                var result = await _cadernoService.DeleteCadernoAsync(cadernoID);
                return Ok(result);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}

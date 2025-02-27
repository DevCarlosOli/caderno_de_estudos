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
    }
}

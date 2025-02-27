using AutoMapper;
using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Domain.Models.DTOs;

namespace CaderDeEstudosAPI.Infra.Mappers {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CadernoDTO, Caderno>()
                .ForMember(dest => dest.CadernoId, opt => opt.Ignore());
            CreateMap<Caderno, CadernoDTO>();
        }
    }
}

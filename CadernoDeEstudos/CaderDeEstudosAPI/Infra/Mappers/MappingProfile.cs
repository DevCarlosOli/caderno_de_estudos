using AutoMapper;
using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Domain.Models.DTOs;

namespace CaderDeEstudosAPI.Infra.Mappers {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CadernoDTO, Caderno>().ForMember(dest => dest.CadernoId, opt => opt.Ignore());
            CreateMap<Caderno, CadernoDTO>();

            CreateMap<NotasDTO, Notas>()
                .ForMember(dest => dest.NotasId, opt => opt.Ignore())
                .ForMember(dest => dest.Caderno, opt => opt.MapFrom(src => src.CadernoDTO));
            CreateMap<Notas, NotasDTO>();
        }
    }
}

using AutoMapper;
using Domain.Models.Models.Domain;
using Domain.Models.Models.PresentationDTO;
using Entities.Entities;

namespace Utility
{
    public class CommandProfile : Profile
    {
        public readonly IMapper Mapper;

        public CommandProfile()
        {
            //CreateMap<TblCommand, Command>();
            //CreateMap<Command, CommandDTO>();
            //CreateMap<TblCommand, CommandDTO>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TblCommand, Command>();
                cfg.CreateMap<Command, CommandDTO>();
                cfg.CreateMap<TblCommand, CommandDTO>();
            });
            Mapper = config.CreateMapper();

        }
    }
}

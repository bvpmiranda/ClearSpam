using AutoMapper;
using ClearSpam.Domain.Entities;

namespace ClearSpam.Application.Models.Mappings
{
    public class FieldMappings : Profile
    {
        public FieldMappings()
        {
            CreateMap<Field, FieldDto>()
                .ReverseMap();
        }
    }
}

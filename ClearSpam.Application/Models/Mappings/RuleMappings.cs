using AutoMapper;
using ClearSpam.Application.Rules.Commands;
using ClearSpam.Domain.Entities;

namespace ClearSpam.Application.Models.Mappings
{
    public class RuleMappings : Profile
    {
        public RuleMappings()
        {
            CreateMap<Rule, RuleDto>()
                .ForMember(x => x.Account, o => o.Ignore())
                .ReverseMap();

            CreateMap<CreateRuleCommand, RuleDto>()
                .ForMember(x => x.Account, o => o.Ignore())
                .ReverseMap();

            CreateMap<UpdateRuleCommand, RuleDto>()
                .ForMember(x => x.Account, o => o.Ignore())
                .ReverseMap();
            
        }
    }
}

using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models;
using MediatR;

namespace ClearSpam.Application.Rules.Queries
{
    public class GetRuleQuery : IEntityDto, IRequest<RuleDto>
    {
        public int Id { get; set; }

        public GetRuleQuery(int id)
        {
            Id = id;
        }
    }
}

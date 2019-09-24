using AutoMapper;
using ClearSpam.Application.BaseMediator.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Rules.Queries
{
    public class GetRuleQueryHandler : GetEntityQueryHandler<Rule, RuleDto>, IRequestHandler<GetRuleQuery, RuleDto>
    {
        public GetRuleQueryHandler(IRepository repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<RuleDto> Handle(GetRuleQuery request, CancellationToken cancellationToken)
        {
            return Handle(request);
        }
    }
}
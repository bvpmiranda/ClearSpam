using AutoMapper;
using ClearSpam.Application.BaseMediator.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Common;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Rules.Queries
{
    public class GetRulesQueryHandler : GetEntitiesQueryHandler<Rule, RuleDto>, IRequestHandler<GetRulesQuery, IEnumerable<RuleDto>>
    {
        public GetRulesQueryHandler(IRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public Task<IEnumerable<RuleDto>> Handle(GetRulesQuery request, CancellationToken cancellationToken)
        {
            var entities = Repository.Get<Rule>(x => x.Account.Id == request.AccountId).OrderBy(x => x.Field, new InvariantCultureComparer()).ThenBy(x => x.Content, new InvariantCultureComparer());
            var result = Mapper.MapList<Rule, RuleDto>(entities);

            return Task.FromResult(result);
        }
    }
}

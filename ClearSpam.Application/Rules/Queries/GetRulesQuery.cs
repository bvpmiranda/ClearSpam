using ClearSpam.Application.Models;
using MediatR;
using System.Collections.Generic;

namespace ClearSpam.Application.Rules.Queries
{
    public class GetRulesQuery : IRequest<IEnumerable<RuleDto>>
    {
        public int AccountId { get; }

        public GetRulesQuery(int accountId)
        {
            AccountId = accountId;
        }
    }
}

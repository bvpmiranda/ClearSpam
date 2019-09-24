using AutoMapper;
using ClearSpam.Application.BaseMediator.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Accounts.Queries
{
    public class GetAccountsQueryHandler : GetEntitiesQueryHandler<Account, AccountDto>, IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
    {
        public GetAccountsQueryHandler(IRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Handle().OrderBy(x => x.Name).AsEnumerable());
        }
    }
}

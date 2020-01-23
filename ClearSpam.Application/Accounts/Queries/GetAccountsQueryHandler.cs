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

namespace ClearSpam.Application.Accounts.Queries
{
    public class GetAccountsQueryHandler : GetEntitiesQueryHandler<Account, AccountDto>, IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
    {
        public GetAccountsQueryHandler(IRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var entities = Repository.Get<Account>(x => x.UserId == request.UserId).OrderBy(x => x.Name);
            var result = Mapper.MapList<Account, AccountDto>(entities);

            return Task.FromResult(result);
        }
    }
}

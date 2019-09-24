using AutoMapper;
using ClearSpam.Application.BaseMediator.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Accounts.Queries
{
    public class GetAccountQueryHandler : GetEntityQueryHandler<Account, AccountDto>, IRequestHandler<GetAccountQuery, AccountDto>
    {
        public GetAccountQueryHandler(IRepository repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<AccountDto> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = Handle(request);
            account.OriginalPassword = account.Password;

            return account;
        }
    }
}
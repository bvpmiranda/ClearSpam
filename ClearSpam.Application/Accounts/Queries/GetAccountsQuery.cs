using ClearSpam.Application.Models;
using MediatR;
using System.Collections.Generic;

namespace ClearSpam.Application.Accounts.Queries
{
    public class GetAccountsQuery : IRequest<IEnumerable<AccountDto>>
    {
        public string UserId { get; set; }

        public GetAccountsQuery(string userId)
        {
            UserId = userId;
        }
    }
}

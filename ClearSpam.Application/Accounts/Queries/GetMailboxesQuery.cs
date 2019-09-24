using ClearSpam.Application.Models;
using MediatR;
using System.Collections.Generic;

namespace ClearSpam.Application.Accounts.Queries
{
    public class GetMailboxesQuery : IRequest<IEnumerable<string>>
    {
        public AccountDto Account { get; set; }

        public GetMailboxesQuery(AccountDto account)
        {
            Account = account;
        }
    }
}

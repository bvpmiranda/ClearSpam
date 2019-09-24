using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models;
using MediatR;

namespace ClearSpam.Application.Accounts.Queries
{
    public class GetAccountQuery : IEntityDto, IRequest<AccountDto>
    {
        public int Id { get; set; }

        public GetAccountQuery(int id)
        {
            Id = id;
        }
    }
}
